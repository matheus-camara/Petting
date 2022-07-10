using Customers.Domain.Contracts;
using FluentValidation;

namespace Customers.Domain.Commands.Update
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        public UpdateCustomerCommandValidator(ICustomerRepository repository) : base()
        {
            _repository = repository;

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (entity, id, token) => await _repository.FindAsync(id, token) is { });

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
