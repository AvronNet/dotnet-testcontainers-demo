using Events.Core.Events;
using Events.Core.Events.Model;
using Events.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;


namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/<EventsController>
        [HttpGet]
        public async Task<IEnumerable<Event>> Get()
        {
            return await _eventService.GetEventsAsync();
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<Event> Get(long id)
        {
            return await _eventService.GetEventByIdAsync(id);
        }

        // POST api/<EventsController>
        [HttpPost]
        public async Task<ActionResult<Event>> Post([FromBody] Event eventToSave)
        {
            try
            {
                return await _eventService.CreateEventAsync(eventToSave);
            }
            catch (BusinessException bex)
            {
                return BadRequest(bex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server error");
            }
            
        }

        // PUT api/<EventsController>/5
        [HttpPut("{id}")]
        public async Task<Event> Put(long id, [FromBody] Event value)
        {
            return await _eventService.UpdateEventAsync(id, value);
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _eventService.DeleteEventAsync(id);
        }
    }
}
