using AutoMapper;
using Events.Core.Events;
using Events.Core.Events.Model;
using Events.Infrastructure.DB.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.DB.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventsDBContext _eventsDBContext;
        private readonly IMapper _mapper;

        public EventsRepository(EventsDBContext eventsDBContext, IMapper mapper)
        {
            _eventsDBContext = eventsDBContext;
            _mapper = mapper;
        }

        public async Task<Event> AddEventAsync(Event eventToAdd)
        {
            var eventEntity = _mapper.Map<Entities.Event>(eventToAdd);
            _eventsDBContext.Events.Add(eventEntity);
            await _eventsDBContext.SaveChangesAsync();
            return _mapper.Map<Event>(eventEntity);
        }

        public async Task DeleteEventAsync(long id)
        {
            var eventToDelete = await _eventsDBContext.Events.FindAsync(id);
            // add check if event exists
            if (eventToDelete == null)
            {
                return;
            }
            _eventsDBContext.Events.Remove(eventToDelete);
            await _eventsDBContext.SaveChangesAsync();
        }

        public async Task DeleteEventByAliasAsync(string alias)
        {
            var eventToDelete = await _eventsDBContext.Events.FirstOrDefaultAsync(e => e.Alias == alias);
            // add check if event exists
            if (eventToDelete == null)
            {
                return;
            }
            _eventsDBContext.Events.Remove(eventToDelete);
            await _eventsDBContext.SaveChangesAsync();
        }

        public async Task<Event> GetEventByIdAsync(long id)
        {
            var eventEntity = await _eventsDBContext.Events.FindAsync(id);
            return _mapper.Map<Event>(eventEntity);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            var eventEntities = await _eventsDBContext.Events.ToListAsync();
            return eventEntities.Select(_mapper.Map<Event>);
        }

        public async Task<Event> UpdateEventAsync(long id, Event eventToUpdate)
        {
            var eventEntity = await _eventsDBContext.Events.FindAsync(id);
            if (eventEntity == null)
            {
                return null;
            }
            _eventsDBContext.Events.Update(eventEntity);
            await _eventsDBContext.SaveChangesAsync();
            return _mapper.Map<Event>(eventEntity);
        }
    }
}
