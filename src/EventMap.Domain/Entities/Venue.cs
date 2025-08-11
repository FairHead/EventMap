namespace EventMap.Domain.Entities;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}