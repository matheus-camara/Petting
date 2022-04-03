using MediatR;
using Notify;

namespace Customer.Domain.Events.Add
{
    public class AddCustumerMessageHandler : INotificationHandler<AddCustomerMessage>
    {
        private readonly INotificationContext context;

        public AddCustumerMessageHandler(INotificationContext context)
        {
            this.context = context;
        }

        public Task Handle(AddCustomerMessage notification, CancellationToken cancellationToken)
        {
            this.context.AddNotification("teste");
            return Task.CompletedTask;
        }
    }
}