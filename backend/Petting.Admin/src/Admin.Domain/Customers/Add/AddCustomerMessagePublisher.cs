using Admin.Domain.MessageBroker;
using Admin.Domain.Publishers;
using MediatR;

namespace Admin.Domain.Customers.Add;

public class AddCustomerMessagePublisher : BasePublisher, IRequestHandler<AddCustomerMessage>
{
    public AddCustomerMessagePublisher(IMessagePublisher eventBus) : base(eventBus)
    {
    }

    public Task<Unit> Handle(AddCustomerMessage request, CancellationToken cancellationToken)
    {
        const string EVENT = "add-customer";
        const string EXCHANGE = $"{EVENT}-exchange";
        EventBus.Publish
        (
            EVENT,
            new AddCustomerEvent
            {
                firstName = request.FirstName,
                lastName = request.LastName
            },
            GetEventId(),
            EXCHANGE
        );

        return Task.FromResult(Unit.Value);
    }
}