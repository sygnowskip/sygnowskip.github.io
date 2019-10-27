using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Common.Database.Creator
{
    public class DatabaseCreator
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseCreator> _logger;

        private DatabaseCreator(string connectionString, ILogger<DatabaseCreator> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task CreateAsync()
        {
            _logger.LogInformation("Dropping and creating database");
            var sqlCommand = AssemblyResources.Get("Common.Database.Creator.Scripts.RecreateDatabase.sql");
            await SqlCommandExecutor.ExecuteAsync(_connectionString.ToMasterDatabase(), string.Format(sqlCommand, _connectionString.Database()));
            _logger.LogInformation("Database successfully created");
        }

        public static DatabaseCreator WithConnectionString(string connectionString, ILogger<DatabaseCreator> logger)
        {
            return new DatabaseCreator(connectionString, logger);
        }
    }
}
