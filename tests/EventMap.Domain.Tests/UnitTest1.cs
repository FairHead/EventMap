using EventMap.Domain.Entities;

namespace EventMap.Domain.Tests;

public class EventTests
{
    [Fact]
    public void Event_Constructor_SetsDefaults()
    {
        // Arrange & Act
        var eventEntity = new Event();

        // Assert
        Assert.Equal(0, eventEntity.Id);
        Assert.Equal(string.Empty, eventEntity.Title);
        Assert.Equal(string.Empty, eventEntity.Description);
        Assert.Empty(eventEntity.Genres);
        Assert.Equal(0.0, eventEntity.Latitude);
        Assert.Equal(0.0, eventEntity.Longitude);
    }

    [Fact]
    public void Event_CanSetProperties()
    {
        // Arrange
        var eventEntity = new Event();
        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddHours(2);

        // Act
        eventEntity.Id = 1;
        eventEntity.Title = "Test Event";
        eventEntity.Description = "Test Description";
        eventEntity.StartUtc = startTime;
        eventEntity.EndUtc = endTime;
        eventEntity.Latitude = 40.7589;
        eventEntity.Longitude = -73.9851;
        eventEntity.Genres = new[] { "Jazz", "Music" };

        // Assert
        Assert.Equal(1, eventEntity.Id);
        Assert.Equal("Test Event", eventEntity.Title);
        Assert.Equal("Test Description", eventEntity.Description);
        Assert.Equal(startTime, eventEntity.StartUtc);
        Assert.Equal(endTime, eventEntity.EndUtc);
        Assert.Equal(40.7589, eventEntity.Latitude);
        Assert.Equal(-73.9851, eventEntity.Longitude);
        Assert.Equal(2, eventEntity.Genres.Length);
        Assert.Contains("Jazz", eventEntity.Genres);
        Assert.Contains("Music", eventEntity.Genres);
    }
}