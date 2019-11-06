using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

namespace NaturalIdentifiers.Persistence
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));

            builder.HasKey(order => order.Id);
            builder.Property(order => order.Id)
                .ValueGeneratedOnAdd();

            builder.Property(order => order.Description);
            builder.OwnsOne(order => order.Customer);
        }
    }

    internal static class PropertyTypeBuilderExtension
    {
        public static PropertyBuilder<T> UseIdentityColumn<T>(this PropertyBuilder<T> builder)
        {
            builder.Metadata.SetAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, SqlServerValueGenerationStrategy.IdentityColumn);
            return builder;
        }
    }
}