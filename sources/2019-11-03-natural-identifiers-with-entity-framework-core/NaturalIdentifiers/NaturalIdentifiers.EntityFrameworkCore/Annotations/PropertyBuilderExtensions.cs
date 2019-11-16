using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NaturalIdentifiers.EntityFrameworkCore.Annotations
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> UseIdentityColumn<T>(this PropertyBuilder<T> builder)
        {
            builder.ValueGeneratedOnAdd();
            builder.Metadata.SetAnnotation(CustomSqlServerAnnotationNames.ValueGenerationStrategy, SqlServerValueGenerationStrategy.IdentityColumn);
            return builder;
        }

        public static SqlServerValueGenerationStrategy GetCustomValueGenerationStrategy(this IProperty property)
        {
            if (property[CustomSqlServerAnnotationNames.ValueGenerationStrategy] != null)
            {
                return (SqlServerValueGenerationStrategy) property[
                    CustomSqlServerAnnotationNames.ValueGenerationStrategy];
            }

            return SqlServerValueGenerationStrategy.None;
        }
    }
}