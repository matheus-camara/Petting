using Microsoft.Extensions.Logging;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Admin.Domain.MessageBroker;

public record MessageBrokerConnection(IConnectionFactory ConnectionFactory, ILogger<MessageBrokerConnection> Logger,
        RetryPolicy RetryPolicy)
    : IMessageBrokerConnection, IDisposable
{
    private readonly object _syncRoot = new();
    private IConnection? _connection;
    private bool _disposed;
    private bool IsConnected => _connection?.IsOpen ?? false && !_disposed;

    public IModel CreateModel()
    {
        return CreateModel(false);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _connection!.ConnectionShutdown -= OnConnectionShutdown;
            _connection!.CallbackException -= OnCallbackException;
            _connection!.ConnectionBlocked -= OnConnectionBlocked;
            _connection!.Dispose();
            _connection = null;
        }
        catch (IOException ex)
        {
            Logger.LogCritical(ex.ToString());
        }
    }

    private IModel CreateModel(bool isRetry)
    {
        if (!IsConnected)
        {
            if (!isRetry)
            {
                TryConnect();
                return CreateModel(true);
            }

            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return _connection!.CreateModel();
    }

    private bool TryConnect()
    {
        Logger.LogInformation("RabbitMQ Client is trying to connect");

        lock (_syncRoot)
        {
            RetryPolicy.Execute(() => { _connection = ConnectionFactory.CreateConnection(); });

            if (IsConnected)
            {
                _connection!.ConnectionShutdown += OnConnectionShutdown;
                _connection!.CallbackException += OnCallbackException;
                _connection!.ConnectionBlocked += OnConnectionBlocked;

                Logger.LogInformation(
                    "RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events",
                    _connection.Endpoint.HostName);

                return true;
            }

            Logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

            return false;
        }
    }

    private void OnConnectionBlocked(object? _, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;

        Logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }

    private void OnCallbackException(object? _, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;

        Logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

        TryConnect();
    }

    private void OnConnectionShutdown(object? _, ShutdownEventArgs reason)
    {
        if (_disposed) return;

        Logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        TryConnect();
    }
}