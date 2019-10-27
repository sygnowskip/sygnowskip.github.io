using System.Data.SqlClient;

namespace Common.Database
{
    public static class ConnectionStringExtensions
    {
        public static string Database(this string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }
        public static string ToMasterDatabase(this string connectionString)
        {
            var database = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            return connectionString.Replace(database, "master");
        }
    }
}