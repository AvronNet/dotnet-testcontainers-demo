using Events.Api.IntegrationTests.Infrastructure;
using Events.Core.Events.Model;
using Events.Infrastructure.DB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Events.Api.IntegrationTests.Testcontainers
{
    public class LocalEnvEventsContollerIT : IClassFixture<LocalEnvWebApplicationFactory<Program>>
    {
        private readonly LocalEnvWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public LocalEnvEventsContollerIT(LocalEnvWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _client = factory.CreateClient();

            // Log the database connection string
            var configuration = _factory.Services.GetRequiredService<IConfiguration>();
            var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
            _output.WriteLine($"dbConnectionString: {dbConnectionString}");
            var redisUrl = configuration.GetConnectionString("RedisUrl");
            _output.WriteLine($"Redis URL: {redisUrl}");

            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<EventsDBContext>();
                var connectionString = dbContext.Database.GetConnectionString();

                _output.WriteLine($"DBContext connection string: {connectionString}");
            }
        }

        [Fact]
        public async Task GetEventsTest()
        {
            var response = await _client.GetAsync("/api/events");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateEventTest()
        {
            var eventContent = new Event
            {
                Name = "Test Event",
                Alias = "Test Event",
                Description = "Test Event Description",
                LogoUrl = "Test Logo Url",
                WebsiteUrl = "Test Website Url",
                OrganizerName = "Test Organizer Name",
                VenueName = "Test Venue",
                VenueAddress = "Test Address",
                VenueCity = "Test City",
                VenueMapsUrl = "Test Maps Url",
                VenueAdditionalDetails = "Test Additional Details",
                EventStartDateTime = DateTime.Now,
                EventEndDateTime = DateTime.Now.AddDays(1),
                RegistrationDeadline = DateTime.Now.AddHours(1)
            };
            var response = await _client.PostAsJsonAsync("/api/events", eventContent);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateEventDraft()
        {
            var eventContent = new Event
            {
                Name = "Test Event",
                Alias = "Test Event",
                Description = "Test Event Description",
                LogoUrl = "Test Logo Url",
                WebsiteUrl = "Test Website Url",
                OrganizerName = "Test Organizer Name",
                VenueName = "Test Venue",
                VenueAddress = "Test Address",
                VenueCity = "Test City",
                VenueMapsUrl = "Test Maps Url",
                VenueAdditionalDetails = "Test Additional Details",
                EventStartDateTime = DateTime.Now,
                EventEndDateTime = DateTime.Now.AddDays(1),
                RegistrationDeadline = DateTime.Now.AddHours(1)
            };
            var response = await _client.PostAsJsonAsync("/api/drafts", eventContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
