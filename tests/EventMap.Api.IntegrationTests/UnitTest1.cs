using Microsoft.AspNetCore.Mvc.Testing;
using EventMap.Shared.DTOs;
using System.Text.Json;

namespace EventMap.Api.IntegrationTests;

public class EventsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public EventsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task GetEvents_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/events");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetEvents_ReturnsEvents()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/events");
        var jsonString = await response.Content.ReadAsStringAsync();
        var events = JsonSerializer.Deserialize<List<EventDto>>(jsonString, _jsonOptions);

        // Assert
        Assert.NotNull(events);
        Assert.NotEmpty(events);
        Assert.All(events, e =>
        {
            Assert.NotEmpty(e.Title);
            Assert.True(e.Id > 0);
        });
    }

    [Fact]
    public async Task GetEvents_WithBoundingBox_FiltersResults()
    {
        // Arrange
        var client = _factory.CreateClient();
        var bbox = "?northEast_Lat=40.800&northEast_Lng=-73.900&southWest_Lat=40.700&southWest_Lng=-74.100";

        // Act
        var response = await client.GetAsync($"/api/events{bbox}");
        var jsonString = await response.Content.ReadAsStringAsync();
        var events = JsonSerializer.Deserialize<List<EventDto>>(jsonString, _jsonOptions);

        // Assert
        Assert.NotNull(events);
        Assert.All(events, e =>
        {
            Assert.True(e.Latitude >= 40.700 && e.Latitude <= 40.800);
            Assert.True(e.Longitude >= -74.100 && e.Longitude <= -73.900);
        });
    }

    [Fact]
    public async Task GetEvent_WithValidId_ReturnsEvent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var eventId = 1;

        // Act
        var response = await client.GetAsync($"/api/events/{eventId}");
        var jsonString = await response.Content.ReadAsStringAsync();
        var eventDto = JsonSerializer.Deserialize<EventDto>(jsonString, _jsonOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(eventDto);
        Assert.Equal(eventId, eventDto.Id);
        Assert.NotEmpty(eventDto.Title);
    }

    [Fact]
    public async Task GetEvents_WithGenreFilter_ReturnsFilteredEvents()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/events?genres=Jazz");
        var jsonString = await response.Content.ReadAsStringAsync();
        var events = JsonSerializer.Deserialize<List<EventDto>>(jsonString, _jsonOptions);

        // Assert
        Assert.NotNull(events);
        Assert.All(events, e => Assert.Contains("Jazz", e.Genres));
    }
}