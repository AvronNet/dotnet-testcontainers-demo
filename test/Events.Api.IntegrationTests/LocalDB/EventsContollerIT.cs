using Events.Api.IntegrationTests.Infrastructure;
using Events.Core.Events.Model;
using Events.Infrastructure.DB.Context;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Events.Api.IntegrationTests.LocalDB
{
    public class EventsContollerIT : IClassFixture<LocalDbWebApplicationFactory<Program>>
    {
        private readonly LocalDbWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public EventsContollerIT(LocalDbWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _client = factory.CreateClient();

            // Log the database connection string
            var configuration = _factory.Services.GetRequiredService<IConfiguration>();
            var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
            _output.WriteLine($"dbConnectionString: {dbConnectionString}");

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

        //[Fact]
        [Fact(Skip = "Not needed")]
        public async Task DeleteEventByAliasTest()
        {
            var alias = "Test Event";
            var response = await _client.DeleteAsync($"/api/events/alias/{alias}");
            response.EnsureSuccessStatusCode();
        }
    }
}
