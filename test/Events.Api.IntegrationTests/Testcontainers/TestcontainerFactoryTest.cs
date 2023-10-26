using Events.Api.IntegrationTests.Infrastructure;
using Events.Infrastructure.DB.Context;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Events.Api.IntegrationTests.Testcontainers
{
    public class TestcontainerFactoryTest : IClassFixture<IntegrationTestFactory<Program, EventsDBContext>>
    {
        private readonly IntegrationTestFactory<Program, EventsDBContext> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;


        public TestcontainerFactoryTest(IntegrationTestFactory<Program, EventsDBContext> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _client = factory.CreateClient();

            // Log the database connection string
            var configuration = _factory.Services.GetRequiredService<IConfiguration>();
            var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
            _output.WriteLine($"Settings connection string: {dbConnectionString}");

            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<EventsDBContext>();
                var connectionString = dbContext.Database.GetConnectionString();

                _output.WriteLine($"DBContext connection string: {connectionString}");
            }
        }

        [Fact]
        public async Task TestcontainersEchoTest()
        {
            var response = await _client.GetAsync("/echo");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestcontainersCustomEchoTest()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<EventsDBContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<TestcontainerFactoryTest>>();

                        try
                        {
                            // Do some custom database initialization here
                            // Utilities.ReinitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding " +
                                "the database with test messages. Error: {Message}",
                                ex.Message);
                        }
                    }
                });
            }).CreateClient();

            var response = await client.GetAsync("/echo");

            response.EnsureSuccessStatusCode();
        }
    }

}
