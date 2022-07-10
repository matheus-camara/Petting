using Customers.Domain.Commands.Add;
using Customers.Domain.Entities;

namespace Customers.Domain.Mappers
{
    internal class AddCustomerCommandMapper : Mapper<AddCustomerCommand, Customer>
    {
        protected override Customer Map(AddCustomerCommand entity)
        {
            return new Customer(entity.FirstName, entity.LastName, entity.Email);
        }
    }
}
