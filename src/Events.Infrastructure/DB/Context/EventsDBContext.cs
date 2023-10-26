using Events.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.DB.Context
{
    public class EventsDBContext : DbContext
    {
        private readonly ILogger<EventsDBContext> _logger;

        public EventsDBContext()
        {
            // note: used only for EF migrations
        }

        public EventsDBContext(DbContextOptions<EventsDBContext> options, ILogger<EventsDBContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Event> Events { get; set; }
    }
}
