using Customers.Domain.Entities;
using Customers.Domain.ValueObjects;
using EntityAbstractions.Persistence.Mappings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Infra.Mappings;

internal class CustomerMapping : AuditableEntityMapping<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(x => x.Value, x => new Email(x));
    }
}