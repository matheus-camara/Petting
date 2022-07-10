using Customers.Domain.ValueObjects;
using FluentValidation;

namespace Customers.Domain.Validators;

public class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);
    }
}