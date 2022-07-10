namespace Customers.Domain.Commands.Add;

public record AddCustomerCommand(string Email, string FirstName, string LastName) : Command;