using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Infrastructure.DB.Entities
{
    [Table("Events")]
    [Index("Alias", Name = "UX_Event_Alias", IsUnique = true)]
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(160)]
        public required string Name { get; set; }
        [StringLength(100)]
        public required string Alias { get; set; }
        [StringLength(4000)]
        public string? Description { get; set; }
        [StringLength(1000)]
        public string? LogoUrl { get; set; }
        [StringLength(1000)]
        public string? WebsiteUrl { get; set; }
        [StringLength(200)]
        public string? OrganizerName { get; set; }
        [StringLength(160)]
        public required string VenueName { get; set; }
        [StringLength(50)]
        public string? VenueCity { get; set; }
        [StringLength(250)]
        public string? VenueAddress { get; set; }
        [StringLength(1000)]
        public string? VenueMapsUrl { get; set; }
        [StringLength(2000)]
        public string? VenueAdditionalDetails { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
