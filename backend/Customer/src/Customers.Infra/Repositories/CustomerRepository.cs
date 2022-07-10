using Customers.Domain.Contracts;
using Customers.Domain.Entities;
using Customers.Domain.ValueObjects;
using EntityAbstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infra.Repositories;

internal class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(DbContext context) : base(context)
    {
    }

    public async Task<Customer?> FindByEmailAsync(Email email, CancellationToken token = default)
    {
        return await FindAsync(x => x.Email == email, token);
    }
}