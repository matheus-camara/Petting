using Customers.Domain.ValueObjects;
using EntityAbstractions;

namespace Customers.Domain.Entities;

public class Customer : AuditableEntity
{
    public Customer(string firstName, string lastName, Email email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public Customer(Guid id, string firstName, string lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void Update(
        string? firstName = null,
        string? lastname = null
    )
    {
        FirstName = firstName ?? FirstName;
        LastName = lastname ?? LastName;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; init; }
}