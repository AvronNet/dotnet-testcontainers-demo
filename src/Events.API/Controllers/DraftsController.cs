using Events.Core.Events.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftsController : ControllerBase
    {
        private readonly IDistributedCache _redis;

        public DraftsController(IDistributedCache redis)
        {
            _redis = redis;
        }

        // GET api/drafts/5
        [HttpGet("{alias}")]
        public async Task<ActionResult<Event>> Get(string alias)
        {
            var jsonData = await _redis.GetStringAsync(alias);
            if (jsonData == null)
                return NotFound();

            return Ok(JsonSerializer.Deserialize<Event>(jsonData));
        }

        // POST api/drafts
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Event eventToSave)
        {
            try
            {
                await _redis.SetStringAsync(eventToSave.Alias, JsonSerializer.Serialize(eventToSave));
            }
            catch (Exception)
            {
                return StatusCode(500, "Server error");
            }
            return Ok();
            
        }

        // PUT api/drafts
        [HttpPut]
        public async Task<Event> Put([FromBody] Event eventToChange)
        {
            await _redis.RemoveAsync(eventToChange.Alias);
            await _redis.SetStringAsync(eventToChange.Alias, JsonSerializer.Serialize(eventToChange));
            return eventToChange;
        }

        // DELETE api/drafts/5
        [HttpDelete("{alias}")]
        public async Task Delete(string alias)
        {
            await _redis.RemoveAsync(alias);
        }
    }
}
