using ProtoBuf;

namespace Admin.Domain.MessageBroker
{
    public interface IMessageListener
    {
        void Subscribe<T>(string eventName, string exchange, Action<T, Action<bool>> onMessage) where T : IExtensible;
    }
}