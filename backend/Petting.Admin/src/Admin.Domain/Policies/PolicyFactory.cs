using System.Net.Sockets;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;

namespace Admin.Domain.Policies;

public static class PolicyFactory
{
    public static RetryPolicy MessageBroker(int retry, Action<Exception, TimeSpan, Context> onError)
    {
        return Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(
                retry,
                count => TimeSpan.FromSeconds(count),
                onError);
    }
}