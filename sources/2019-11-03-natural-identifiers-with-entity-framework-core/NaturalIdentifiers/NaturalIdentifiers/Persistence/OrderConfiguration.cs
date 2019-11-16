using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaturalIdentifiers.EntityFrameworkCore.Annotations;

namespace NaturalIdentifiers.Persistence
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));

            builder.HasKey(order => order.Id);
            builder.Property(order => order.Id)
                .UseIdentityColumn();

            builder.Property(order => order.Description);
            builder.OwnsOne(order => order.Customer);
        }
    }
}