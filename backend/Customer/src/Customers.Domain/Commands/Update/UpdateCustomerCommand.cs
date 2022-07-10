namespace Customers.Domain.Commands.Update;
public record UpdateCustomerCommand(
    string FirstName,
    string LastName) : Command
{
    internal Guid CustomerId { get; private set; }

    public UpdateCustomerCommand WithId(Guid id)
    {
        CustomerId = id;
        return this;
    }
}
