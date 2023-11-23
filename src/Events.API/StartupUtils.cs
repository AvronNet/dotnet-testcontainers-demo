using DotNet.Testcontainers.Builders;
using Events.Infrastructure.DB.Context;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Events.Api
{
    public static class StartupUtils
    {
        public static void ApplyMigrations(IHost host)
        {
            // For more information review https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
            using var scope = host.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                Console.WriteLine("ApplyMigrations: Starting migration for EventsApi");
                var apiDb = scope.ServiceProvider.GetRequiredService<EventsDBContext>();
                apiDb.Database.Migrate();
                Console.WriteLine("ApplyMigrations: Finished migration for EventsApi");
            }
            catch (Exception e)
            {
                logger.LogError(e, "ApplyMigrations: Migration Error");
                throw;
            }
        }

        /// <summary>
        /// Use Postgres Testcontainer for Local environment
        /// </summary>
        public static void StartTestcontainerForLocalDev()
        {
            #region Use MSSQL Testcontainer for Local environment

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Local")
            {
                // Setup testcontainers
                // MSSQL Database
                var mssqlContainer = new MsSqlBuilder()
                    .WithWaitStrategy(Wait.ForWindowsContainer())
                    .WithCleanUp(true)
                    .Build();

                mssqlContainer.StartAsync().Wait();

                var containerWait = Environment.GetEnvironmentVariable("IT_TESTCONTAINER_WAIT_TIME");
                if (containerWait != null && int.TryParse(containerWait, out int waitMilis))
                {
                    Thread.Sleep(waitMilis);
                }

                System.Diagnostics.Debug.WriteLine("Testcontainer MSSQL connection string: " + mssqlContainer.GetConnectionString());
                Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", mssqlContainer.GetConnectionString());

                // Redis
                var redisContainer = new RedisBuilder()
                    .WithWaitStrategy(Wait.ForUnixContainer())
                    .WithCleanUp(true)
                    .Build();

                redisContainer.StartAsync().Wait();

                System.Diagnostics.Debug.WriteLine("Testcontainer Redis connection string: " + redisContainer.GetConnectionString());
                Environment.SetEnvironmentVariable("ConnectionStrings__RedisUrl", redisContainer.GetConnectionString());
            }

            #endregion
        }
    }
}
