using EventMap.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventMap.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;

    public EventsController(ILogger<EventsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<EventDto>> GetEvents([FromQuery] EventsQueryRequest request)
    {
        _logger.LogInformation("Getting events with bbox: NE({NorthEast_Lat}, {NorthEast_Lng}), SW({SouthWest_Lat}, {SouthWest_Lng})",
            request.NorthEast_Lat, request.NorthEast_Lng, request.SouthWest_Lat, request.SouthWest_Lng);

        // Mock data for MVP
        var mockEvents = new List<EventDto>
        {
            new EventDto
            {
                Id = 1,
                Title = "Jazz Night at Central Park",
                Description = "Free outdoor jazz concert featuring local artists",
                StartUtc = DateTime.UtcNow.AddDays(1),
                EndUtc = DateTime.UtcNow.AddDays(1).AddHours(3),
                Latitude = 40.7829,
                Longitude = -73.9654,
                Genres = new[] { "Jazz", "Music" },
                Venue = new VenueDto
                {
                    Id = 1,
                    Name = "Central Park Bandshell",
                    Address = "Central Park, New York, NY",
                    Latitude = 40.7829,
                    Longitude = -73.9654,
                    Description = "Outdoor performance venue in Central Park"
                }
            },
            new EventDto
            {
                Id = 2,
                Title = "Street Art Festival",
                Description = "Local street artists showcase their work",
                StartUtc = DateTime.UtcNow.AddDays(2),
                EndUtc = DateTime.UtcNow.AddDays(2).AddHours(6),
                Latitude = 40.7614,
                Longitude = -73.9776,
                Genres = new[] { "Art", "Street Art" }
            },
            new EventDto
            {
                Id = 3,
                Title = "Acoustic Coffee Session",
                Description = "Intimate acoustic performances with coffee",
                StartUtc = DateTime.UtcNow.AddHours(6),
                EndUtc = DateTime.UtcNow.AddHours(8),
                Latitude = 40.7505,
                Longitude = -73.9934,
                Genres = new[] { "Acoustic", "Music", "Coffee" },
                Venue = new VenueDto
                {
                    Id = 2,
                    Name = "Brooklyn Coffee House",
                    Address = "123 Brooklyn Ave, Brooklyn, NY",
                    Latitude = 40.7505,
                    Longitude = -73.9934,
                    Description = "Cozy coffee house with live music"
                }
            }
        };

        // Apply bounding box filter if provided
        if (request.NorthEast_Lat.HasValue && request.NorthEast_Lng.HasValue &&
            request.SouthWest_Lat.HasValue && request.SouthWest_Lng.HasValue)
        {
            mockEvents = mockEvents.Where(e =>
                e.Latitude >= request.SouthWest_Lat.Value &&
                e.Latitude <= request.NorthEast_Lat.Value &&
                e.Longitude >= request.SouthWest_Lng.Value &&
                e.Longitude <= request.NorthEast_Lng.Value).ToList();
        }

        // Apply genre filter if provided
        if (request.Genres?.Length > 0)
        {
            mockEvents = mockEvents.Where(e =>
                e.Genres.Any(g => request.Genres.Contains(g, StringComparer.OrdinalIgnoreCase))).ToList();
        }

        // Apply time filters if provided
        if (request.StartAfter.HasValue)
        {
            mockEvents = mockEvents.Where(e => e.StartUtc >= request.StartAfter.Value).ToList();
        }

        if (request.StartBefore.HasValue)
        {
            mockEvents = mockEvents.Where(e => e.StartUtc <= request.StartBefore.Value).ToList();
        }

        return Ok(mockEvents);
    }

    [HttpGet("{id}")]
    public ActionResult<EventDto> GetEvent(int id)
    {
        _logger.LogInformation("Getting event with id: {EventId}", id);

        // Mock single event data
        var mockEvent = new EventDto
        {
            Id = id,
            Title = $"Event {id}",
            Description = $"Description for event {id}",
            StartUtc = DateTime.UtcNow.AddDays(1),
            EndUtc = DateTime.UtcNow.AddDays(1).AddHours(2),
            Latitude = 40.7589 + (id * 0.001),
            Longitude = -73.9851 + (id * 0.001),
            Genres = new[] { "Music", "Live" }
        };

        return Ok(mockEvent);
    }
}