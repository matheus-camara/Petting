using Customers.Domain.Commands.Handlers;
using Customers.Domain.Contracts;
using Customers.Domain.Entities;
using Customers.Domain.Mappers;
using Paramore.Brighter;

namespace Customers.Domain.Commands.Add;

public class AddCustomerCommandHandler : RequestHandlerAsync<AddCustomerCommand>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper<AddCustomerCommand, Customer> _mapper;

    public AddCustomerCommandHandler(ICustomerRepository repository, IMapper<AddCustomerCommand, Customer> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [Pipeline(1, typeof(LoggingHandler<>))]
    [Pipeline(2, typeof(ValidationHandler<>))]
    public override Task<AddCustomerCommand> HandleAsync(AddCustomerCommand command,
        CancellationToken cancellationToken)
    {
        _repository.AddAsync(_mapper.MapFrom(command)!);
        return base.HandleAsync(command, cancellationToken);
    }
}