using Events.Core.Events.Model;
using Events.Core.Exceptions;

namespace Events.Core.Events
{
    public class EventService
    {
        private readonly IEventsRepository _eventsRepository;

        public EventService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<Event> GetEventByIdAsync(long id)
        {
            return await _eventsRepository.GetEventByIdAsync(id);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _eventsRepository.GetEventsAsync();
        }

        public async Task<Event> CreateEventAsync(Event eventToAdd)
        {
            try
            {
                return await _eventsRepository.AddEventAsync(eventToAdd);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate key")
                    || (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key")))
                {
                    throw new BusinessException("Duplicate alias, please use a unique alias for your event.");
                }
                throw;
            }
        }

        public async Task<Event> UpdateEventAsync(long id, Event eventToUpdate)
        {
            return await _eventsRepository.UpdateEventAsync(id, eventToUpdate);
        }

        public async Task DeleteEventAsync(long id)
        {
            await _eventsRepository.DeleteEventAsync(id);
        }

        public async Task DeleteEventByAliasAsync(string alias)
        {
            await _eventsRepository.DeleteEventByAliasAsync(alias);
        }
    }
}
