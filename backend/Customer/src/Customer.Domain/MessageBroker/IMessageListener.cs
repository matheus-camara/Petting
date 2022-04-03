using ProtoBuf;
using RabbitMQ.Client;

namespace Customer.Domain.MessageBroker
{
    public interface IMessageListener
    {
        IModel Subscribe<T>(string eventName, string exchange, Func<T, Action<bool>, Task> onMessage) where T : IExtensible;
    }
}