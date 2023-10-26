using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace Events.Api.IntegrationTests.Infrastructure
{
    public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : DbContext
    {
        private readonly MsSqlContainer _container;

        public IntegrationTestFactory()
        {
            _container = new MsSqlBuilder()
                .WithCleanUp(true)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContext<TDbContext>(
                options => { options.UseSqlServer(_container.GetConnectionString()); },
                ServiceLifetime.Transient);
            services.EnsureDbCreated<TDbContext>();

            // Creator repositories
            // services.AddScoped<IEventsCreatorRepository, EventsCreatorRepository>();

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<IntegrationTestFactory<TProgram, TDbContext>>>();

                try
                {
                    // optionally seed database here
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            }

        }

        public async Task InitializeAsync() => await _container.StartAsync();

        public new async Task DisposeAsync() => await _container.DisposeAsync();
    }
}
