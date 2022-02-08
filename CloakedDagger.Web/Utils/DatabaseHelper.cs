using Microsoft.Extensions.Configuration;

namespace CloakedDagger.Web.Utils
{
    public static class DatabaseHelper
    {
        public static string GetDatabaseConnectionString(this IConfiguration configuration)
        {
            var databaseHost = configuration.GetSection("database:host").Value;
            var databaseUser = configuration.GetSection("database:user").Value;
            var databasePassword = configuration.GetSection("database:password").Value;
            var databasePort = configuration.GetSection("database:port").Value;
            var databaseSchema = configuration.GetSection("database:schema").Value;
            var sslMode = configuration.GetSection("database:sslMode").Value;
            
            return $"Host={databaseHost};Username={databaseUser};Password='{databasePassword}';Port={databasePort};Database={databaseSchema};sslmode={sslMode};Trust Server Certificate=true";
        }
    }
}