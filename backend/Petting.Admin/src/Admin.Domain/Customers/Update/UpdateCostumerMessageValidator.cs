using FluentValidation;

namespace Admin.Domain.Customers.Update;

public class UpdateCostumerMessageValidator : AbstractValidator<UpdateCostumerMessage>
{
    public UpdateCostumerMessageValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}