using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace NaturalIdentifiers.EntityFrameworkCore.Annotations
{
    public class CustomAnnotationProvider : SqlServerMigrationsAnnotationProvider
    {
        public CustomAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies) : base(dependencies)
        {
        }

        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            var baseAnnotations = base.For(property);

            if (property.GetCustomValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn)
            {
                var seed = property.GetIdentitySeed();
                var increment = property.GetIdentityIncrement();

                var identityInsert = new Annotation(
                    SqlServerAnnotationNames.Identity,
                    string.Format(CultureInfo.InvariantCulture, "{0}, {1}", seed ?? 1, increment ?? 1));

                return baseAnnotations.Concat(new[] { identityInsert });
            }

            return baseAnnotations;
        }
    }
}