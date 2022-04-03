using Customer.Domain.MessageBroker;
using Customer.Domain.Policies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;

namespace Customer.Domain.Config;

public static class MessageBroker
{
    private const string COULD_NOT_CONNECT = "Could not connect after {TimeOut}s ({ExceptionMessage})";

    public static IServiceCollection AddMessageBroker(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddSingleton<IConnectionFactory>(x => new ConnectionFactory
        {
            Uri = new Uri(configuration.GetValue<string>("rabbitmq:url")),
            UserName = configuration.GetValue<string>("rabbitmq:user"),
            Password = configuration.GetValue<string>("rabbitmq:password")
        });
        collection.AddSingleton<IMessageBrokerConnection, MessageBrokerConnection>();
        collection.AddSingleton<IMessageListener, EventBus>();
        collection.AddSingleton(p => PolicyFactory.MessageBroker(3, OnRetryForMessageBrokerConnection()));

        return collection;
    }

    private static Action<Exception, TimeSpan, Context> OnRetryForMessageBrokerConnection()
    {
        return (ex, time, ctx) =>
        {
            if (!ctx.TryGetLogger(out var logger)) return;

            logger!.LogWarning(ex, COULD_NOT_CONNECT, $"{time.TotalSeconds:n1}", ex.Message);
        };
    }
}

public static class PollyContextExtensions
{
    public static bool TryGetLogger(this Context context, out ILogger? logger)
    {
        if (context.TryGetValue("logger", out var loggerObject) && loggerObject is ILogger theLogger)
        {
            logger = theLogger;
            return true;
        }

        logger = null;
        return false;
    }
}