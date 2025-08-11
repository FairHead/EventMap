namespace EventMap.Shared.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartUtc { get; set; }
    public DateTime? EndUtc { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string[] Genres { get; set; } = Array.Empty<string>();
    public VenueDto? Venue { get; set; }
}