using System;
using MySql.Data.MySqlClient;
using Serilog;

namespace CloakedDagger.Web.Database
{
    public class DatabaseMigrator
    {
        public static void MigrateDatabase(string connectionString)
        {
            try
            {
                Log.Information("Migrating our database");
                var databaseConnection = new MySqlConnection(connectionString);
                var evolve = new Evolve.Evolve(databaseConnection, Log.Information)
                {
                    Locations = new[]
                    {
                        "Database/Migrations"
                    },
                    IsEraseDisabled = true,
                    EnableClusterMode = true
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Fatal("Database migrations failed.", ex);
                throw;
            }
        }
    }
}