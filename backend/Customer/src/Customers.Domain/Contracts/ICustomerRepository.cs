using Customers.Domain.Entities;
using Customers.Domain.ValueObjects;

namespace Customers.Domain.Contracts;

public interface ICustomerRepository
{
    void Update(Customer Customers);
    void Delete(Customer Customers);
    Task AddAsync(Customer Customer);
    ValueTask<Customer?> FindAsync(Guid id, CancellationToken token = default);
    Task<Customer?> FindByEmailAsync(Email email, CancellationToken token = default);
    Task<IList<Customer>> GetAsync(int skip, int take, CancellationToken token = default);
}