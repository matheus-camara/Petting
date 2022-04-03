using RabbitMQ.Client;

namespace Customer.Domain.MessageBroker;

public interface IMessageBrokerConnection : IDisposable
{
    IModel CreateModel();
}