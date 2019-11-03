using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NaturalIdentifiers.Persistence
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer));

            builder.HasKey(customer => customer.Id);

            builder.Property(customer => customer.FirstName);
            builder.Property(customer => customer.LastName);

            builder.OwnsOne(customer => customer.Address);
        }
    }
}