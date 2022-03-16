using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using ProtoBuf;
using RabbitMQ.Client;

namespace Admin.Domain.MessageBroker;

public class EventBus : IDisposable, IMessagePublisher
{
    private readonly IMessageBrokerConnection _connection;
    private readonly ILogger<EventBus> _logger;
    private readonly RetryPolicy _retryPolicy;

    public EventBus(IMessageBrokerConnection connection, ILogger<EventBus> logger, RetryPolicy retryPolicy)
    {
        _connection = connection;
        _logger = logger;
        _retryPolicy = retryPolicy;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public void Publish(string eventName, IExtensible message, string eventId, string exchange)
    {
        using var _channel = _connection.CreateModel();
        const int PERSISTENT = 2;
        DeclareExchange(eventName, eventId, exchange, _channel);
        _retryPolicy.Execute(ctx =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.DeliveryMode = PERSISTENT;

                _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", eventId);

                _channel.BasicPublish(
                    exchange,
                    eventName,
                    true,
                    properties,
                    GetBuffer(message));
            },
            GetPolicyContext());
    }

    private Context GetPolicyContext()
    {
        var ctx = new Context();
        ctx.Add("logger", _logger);
        return ctx;
    }

    private ReadOnlyMemory<byte> GetBuffer(IExtensible message)
    {
        using var messageStream = new MemoryStream();
        Serializer.Serialize(messageStream, message);
        ArraySegment<byte> messageBuffer;

        if (messageStream.TryGetBuffer(out messageBuffer))
            return messageBuffer;

        return messageStream.ToArray();
    }

    private void DeclareExchange(string eventName, string eventId, string exchange, IModel _channel)
    {
        _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", eventId, eventName);
        _channel.ExchangeDeclare(exchange, "direct");
    }
}