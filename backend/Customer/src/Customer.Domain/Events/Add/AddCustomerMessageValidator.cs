using FluentValidation;

namespace Customer.Domain.Events.Add;

public class AddCustomerMessageValidator : AbstractValidator<AddCustomerMessage>
{
    public AddCustomerMessageValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}