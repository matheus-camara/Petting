using Admin.Domain.MessageBroker;
using MediatR;

namespace Admin.Domain.Customers.Add;

public class AddCostumerMessage : Message, IRequest
{
    public AddCostumerMessage(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}