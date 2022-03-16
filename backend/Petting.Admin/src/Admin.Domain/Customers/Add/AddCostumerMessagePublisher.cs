using Admin.Domain.MessageBroker;
using Admin.Domain.Publishers;
using MediatR;

namespace Admin.Domain.Customers.Add;

public class AddCostumerMessagePublisher : BasePublisher, IRequestHandler<AddCostumerMessage>
{
    public AddCostumerMessagePublisher(IMessagePublisher eventBus) : base(eventBus)
    {
    }

    public Task<Unit> Handle(AddCostumerMessage request, CancellationToken cancellationToken)
    {
        const string EVENT = "add-costumer";
        const string EXCHANGE = $"{EVENT}-exchange";
        EventBus.Publish
        (
            EVENT,
            new AddCostumerEvent
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