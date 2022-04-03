using MediatR;

namespace Admin.Domain.Customers.Update;

public record UpdateCustomerMessage
(
    string FirstName,
    string LastName
) : IRequest;