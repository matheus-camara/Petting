using FluentValidation;

namespace Admin.Domain.Customers.Update;

public class UpdateCustomerMessageValidator : AbstractValidator<UpdateCustomerMessage>
{
    public UpdateCustomerMessageValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}