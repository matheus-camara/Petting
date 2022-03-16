using FluentValidation;

namespace Admin.Domain.Customers.Add;

public class AddCostumerMessageValidator : AbstractValidator<AddCostumerMessage>
{
    public AddCostumerMessageValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}