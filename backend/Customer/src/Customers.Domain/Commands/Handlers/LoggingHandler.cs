using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Customers.Domain.Commands.Handlers;

internal class LoggingHandler<T> : RequestHandlerAsync<T> where T : Command
{
    private static readonly string EVENT_TYPE = nameof(T);
    private readonly ILogger<LoggingHandler<T>> _logger;

    public LoggingHandler(ILogger<LoggingHandler<T>> logger)
    {
        _logger = logger;
    }

    public override Task<T> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Execution begin >>> {type}", EVENT_TYPE);

        var output = base.HandleAsync(command);

        _logger.LogInformation("Execution end <<< {type}", EVENT_TYPE);

        return output;
    }
}