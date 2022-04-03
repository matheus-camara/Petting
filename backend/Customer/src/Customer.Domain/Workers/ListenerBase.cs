using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customer.Domain.MessageBroker;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notify;
using ProtoBuf;

namespace Customer.Domain.Workers
{
    public abstract class ListenerBase<T> : BackgroundService where T : IExtensible
    {
        private IServiceScopeFactory _serviceScopeFactory;
        protected IMessageListener Listener { get; init; }
        protected string EventName { get; init; }
        protected string Exchange { get; init; }

        protected ListenerBase(IServiceScopeFactory serviceScopeFactory, IMessageListener listener, string eventName, string exchange)
        {
            _serviceScopeFactory = serviceScopeFactory;
            Listener = listener;
            EventName = eventName;
            Exchange = exchange;
        }
        protected abstract Task Handle(INotificationContext context, IMediator mediator, T message, Action<bool> handled);

        private Task Handle(T message, Action<bool> handle)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var context = scope.ServiceProvider.GetRequiredService<INotificationContext>();
                return Handle(context, mediator, message, handle);
            }
        }

        protected sealed override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = Listener.Subscribe<T>(EventName, Exchange, Handle);
            return Task.CompletedTask;
        }
    }
}