using Admin.Domain.MessageBroker;
using MediatR;

namespace Admin.Domain.Customers.Add;

public class AddCustomerMessage : IRequest
{
    public AddCustomerMessage(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}