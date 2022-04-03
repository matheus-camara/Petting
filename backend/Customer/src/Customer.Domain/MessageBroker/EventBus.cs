using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Customer.Domain.MessageBroker;

public class EventBus : IDisposable, IMessagePublisher, IMessageListener
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
        DeclareExchange(eventName, exchange, _channel);
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

    public IModel Subscribe<T>(string eventName, string exchange, Func<T, Action<bool>, Task> onMessage) where T : IExtensible
    {
        var channel = _connection.CreateModel();
        DeclareExchange(eventName, exchange, channel);

        var queue = channel.QueueDeclare(eventName, true, false, false);

        channel.QueueBind(queue.QueueName, exchange, queue.QueueName);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, message) =>
        {
            var @event = Serializer.Deserialize<T>(message.Body);
            onMessage.Invoke(@event, (sucess) => Ack(message, sucess));
        };

        var result = channel.BasicConsume(
            queue: queue.QueueName,
            autoAck: false,
            consumer: consumer);

        return channel;
    }

    private void Ack(BasicDeliverEventArgs message, bool sucess)
    {
        var channel = _connection.CreateModel();
        if (!sucess && !message.Redelivered)
        {
            channel.BasicNack(message.DeliveryTag, multiple: false, requeue: true);
            return;
        }
        else if (!sucess && message.Redelivered)
        {
            channel.BasicReject(message.DeliveryTag, requeue: false);
        }

        channel.BasicAck(message.DeliveryTag, multiple: false);
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

    private void DeclareExchange(string eventName, string exchange, IModel _channel)
    {
        _logger.LogTrace("Creating RabbitMQ channel to publish event: ({EventName})", eventName);
        _channel.ExchangeDeclare(exchange, "fanout");
    }
}