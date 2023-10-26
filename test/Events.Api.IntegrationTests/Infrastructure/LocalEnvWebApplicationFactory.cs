using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.Configuration;

namespace Events.Api.IntegrationTests.Infrastructure
{
    public class LocalEnvWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set Docker environment
            builder.UseEnvironment("Local");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local");
            Environment.SetEnvironmentVariable("IT_TESTCONTAINER_WAIT_TIME", "3000");
        }
    }
}
