using FluentValidation;
using Notify;
using Paramore.Brighter;

namespace Customers.Domain.Commands.Handlers;

internal class ValidationHandler<T> : RequestHandlerAsync<T> where T : Command
{
    private readonly INotificationContext _notificationContext;
    private readonly IValidator<T> _validator;

    public ValidationHandler(IValidator<T> validator, INotificationContext notificationContext)
    {
        _validator = validator;
        _notificationContext = notificationContext;
    }

    public override async Task<T> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        foreach (var error in result.Errors)
        {
            _notificationContext.AddNotification(error.PropertyName, error.ErrorMessage);
            return command;
        }

        return await base.HandleAsync(command, cancellationToken);
    }
}