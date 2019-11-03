using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Common.Docker.Resources
{
    public class MsSqlResourceAwaiter
    {
        private readonly ILogger<MsSqlResourceAwaiter> _logger;

        public MsSqlResourceAwaiter(ILogger<MsSqlResourceAwaiter> logger)
        {
            _logger = logger;
        }

        public async Task WaitForConnectionAsync(string connectionString)
        {
            var acceptingConnections = false;
            while (!acceptingConnections)
            {
                try
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        await sqlConnection.OpenAsync();
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            acceptingConnections = true;
                            _logger.LogInformation($"Connection accepted! ({connectionString})");
                        }
                    }
                }
                catch
                {
                    acceptingConnections = false;
                    _logger.LogInformation($"Connection refused, retrying... ({connectionString})");
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}