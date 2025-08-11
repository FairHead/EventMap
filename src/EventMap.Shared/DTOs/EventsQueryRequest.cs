namespace EventMap.Shared.DTOs;

public class EventsQueryRequest
{
    public double? NorthEast_Lat { get; set; }
    public double? NorthEast_Lng { get; set; }
    public double? SouthWest_Lat { get; set; }
    public double? SouthWest_Lng { get; set; }
    public string[]? Genres { get; set; }
    public DateTime? StartAfter { get; set; }
    public DateTime? StartBefore { get; set; }
}