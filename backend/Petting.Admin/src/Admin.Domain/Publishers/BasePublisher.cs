using Admin.Domain.MessageBroker;

namespace Admin.Domain.Publishers;

public abstract class BasePublisher
{
    protected BasePublisher(IMessagePublisher eventBus)
    {
        EventBus = eventBus;
    }

    protected IMessagePublisher EventBus { get; init; }


    protected string GetEventId()
    {
        return Guid.NewGuid().ToString();
    }
}