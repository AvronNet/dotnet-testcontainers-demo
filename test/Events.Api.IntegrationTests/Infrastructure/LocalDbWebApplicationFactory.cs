using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.Configuration;

namespace Events.Api.IntegrationTests.Infrastructure
{
    public class LocalDbWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set Docker environment
            builder.UseEnvironment("Development");
        }
    }
}
