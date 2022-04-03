using FluentValidation;

namespace Admin.Domain.Customers.Add;

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