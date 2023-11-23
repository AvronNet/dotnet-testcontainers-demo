using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Events.Api.IntegrationTests.Infrastructure
{
    public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : DbContext
    {
        private readonly MsSqlContainer _msSqlContainer;
        private readonly RedisContainer _redisContainer;

        public IntegrationTestFactory()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithCleanUp(true)
                .Build();
            _redisContainer = new RedisBuilder()
                .WithImage("redis:7.0")
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
                options => { options.UseSqlServer(_msSqlContainer.GetConnectionString()); },
                ServiceLifetime.Transient);
            services.EnsureDbCreated<TDbContext>();

            var redisService = services.FirstOrDefault(d => d.ServiceType == typeof(IDistributedCache));
            if (redisService != null) { services.Remove(redisService); }
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });

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

        public async Task InitializeAsync() {
            await _msSqlContainer.StartAsync();
            await _redisContainer.StartAsync();
    }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await _redisContainer.DisposeAsync();
        }
    }
}
