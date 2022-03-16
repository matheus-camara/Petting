using RabbitMQ.Client;

namespace Admin.Domain.MessageBroker;

public interface IMessageBrokerConnection : IDisposable
{
    IModel CreateModel();
}