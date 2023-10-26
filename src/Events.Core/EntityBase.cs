namespace Events.Core
{
    public class EntityBase
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}