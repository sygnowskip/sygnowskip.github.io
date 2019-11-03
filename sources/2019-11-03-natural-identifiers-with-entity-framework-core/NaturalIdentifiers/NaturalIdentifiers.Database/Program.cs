using Common.Database;
using DbUp;
using Microsoft.Extensions.Logging;

namespace NaturalIdentifiers.Database
{
    public class DatabaseUpgrader
    {
        public static bool Execute(string connectionString, ILogger<DatabaseUpgrader> logger)
        {
            return DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseUpgrader).Assembly)
                .LogTo(UpgradeLog<DatabaseUpgrader>.CreateFor(logger))
                .Build()
                .PerformUpgrade()
                .Successful;
        }
    }
}
