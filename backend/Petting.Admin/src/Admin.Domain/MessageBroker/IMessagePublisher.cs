using ProtoBuf;

namespace Admin.Domain.MessageBroker;

public interface IMessagePublisher
{
    void Publish(string eventName, IExtensible message, string eventId, string exchange);
}