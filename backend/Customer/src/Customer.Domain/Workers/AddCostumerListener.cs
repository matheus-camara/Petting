using Customer.Domain.MessageBroker;
using MediatR;
using Customer.Domain.Events.Add;
using Microsoft.Extensions.DependencyInjection;
using Notify;

namespace Customer.Domain.Workers
{
    public class AddCostumerListener : ListenerBase<AddCustomerEvent>
    {
        private const string EVENT = "add-customer";
        private const string EXCHANGE = $"{EVENT}-exchange";
        public AddCostumerListener(IServiceScopeFactory serviceScopeFactory, IMessageListener listener)
            : base(serviceScopeFactory, listener, EVENT, EXCHANGE)
        {
        }
        protected override async Task Handle(INotificationContext context, IMediator mediator, AddCustomerEvent message, Action<bool> handled)
        {
            await mediator.Publish(new AddCustomerMessage(message.firstName, message.lastName));
            handled(context.IsEmpty);
        } 
    }
}