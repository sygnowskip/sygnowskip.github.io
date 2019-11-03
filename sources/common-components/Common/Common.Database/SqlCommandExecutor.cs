using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Common.Database
{
    public static class SqlCommandExecutor
    {
        public static async Task ExecuteAsync(string connectionString, string sqlCommand, int timeoutInSeconds = 30)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandTimeout = timeoutInSeconds;
                    command.CommandText = sqlCommand;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}