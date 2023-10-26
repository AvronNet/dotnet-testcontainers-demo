using Events.Core.Events.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Core.Events
{
    public interface IEventsRepository
    {
        Task<Event> GetEventByIdAsync(long id);
        Task<IEnumerable<Event>> GetEventsAsync();
        Task<Event> AddEventAsync(Event eventToAdd);
        Task<Event> UpdateEventAsync(long id, Event eventToUpdate);
        Task DeleteEventAsync(long id);
    }
}
