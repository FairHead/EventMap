namespace EventMap.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartUtc { get; set; }
    public DateTime? EndUtc { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string[] Genres { get; set; } = Array.Empty<string>();
    public int? VenueId { get; set; }
    public virtual Venue? Venue { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}
