using Customers.Domain.Commands.Handlers;
using Customers.Domain.Contracts;
using Paramore.Brighter;

namespace Customers.Domain.Commands.Update
{
    public class UpdateCustomerCommandHandler : RequestHandlerAsync<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _repository = customerRepository;
        }

        [Pipeline(1, typeof(LoggingHandler<>))]
        [Pipeline(2, typeof(ValidationHandler<>))]
        public override async Task<UpdateCustomerCommand> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
        {
            var customer = await _repository.FindAsync(command.CustomerId);
            customer!.Update(command.FirstName, command.LastName);
            _repository.Update(customer);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
