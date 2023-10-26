
namespace Events.Core.Events.Model
{
    public class Event : EntityBase
    {
        public required string Name { get; set; }
        public required string Alias { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? OrganizerName { get; set; }

        public required string VenueName { get; set; }
        public string? VenueCity { get; set; }
        public string? VenueAddress { get; set; }
        public string? VenueMapsUrl { get; set; }
        public string? VenueAdditionalDetails { get; set; }
        
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public DateTime RegistrationDeadline { get; set; }


    }
}
