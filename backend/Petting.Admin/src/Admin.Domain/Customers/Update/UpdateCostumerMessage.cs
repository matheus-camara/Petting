using MediatR;

namespace Admin.Domain.Customers.Update;

public record UpdateCostumerMessage
(
    string FirstName,
    string LastName
) : IRequest;