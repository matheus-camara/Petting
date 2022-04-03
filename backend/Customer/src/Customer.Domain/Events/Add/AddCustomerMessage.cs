using MediatR;

namespace Customer.Domain.Events.Add;

public class AddCustomerMessage : INotification
{
    public AddCustomerMessage(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}