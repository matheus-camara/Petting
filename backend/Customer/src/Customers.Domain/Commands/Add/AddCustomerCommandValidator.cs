using Customers.Domain.Contracts;
using Customers.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Customers.Domain.Commands.Add;

public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    private ICustomerRepository _repository;
    private IStringLocalizer<Resources> _localizer;

    public AddCustomerCommandValidator(ICustomerRepository repository, IStringLocalizer<Resources> localizer)
    {
        _repository = repository;
        _localizer = localizer;

        RuleFor(x => x.FirstName)
               .NotEmpty()
               .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotNull()
            .EmailAddress()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Email)
                        .MustAsync(async (entity, email, token) => await _repository.FindByEmailAsync(email, token) is not { })
                        .WithMessage(_localizer.GetString(Resources.EmailAlreadyInUse));
                });
    }
}