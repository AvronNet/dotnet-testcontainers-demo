using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EchoController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            var bodyAsText = await new StreamReader(Request.Body).ReadToEndAsync();

            // return echo of the sent request
            return Ok(bodyAsText);
        }
    }
}
