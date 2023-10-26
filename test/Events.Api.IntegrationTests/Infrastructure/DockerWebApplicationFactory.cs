using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.Configuration;

namespace Events.Api.IntegrationTests.Infrastructure
{
    public class DockerWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    // Set environment specific settings
            //    config.AddJsonFile("appsettings.docker.json");
            //});

            // Set Docker environment
            builder.UseEnvironment("Docker");
        }
    }
}
