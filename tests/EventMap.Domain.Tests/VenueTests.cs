using EventMap.Domain.Entities;

namespace EventMap.Domain.Tests;

public class VenueTests
{
    [Fact]
    public void Venue_Constructor_SetsDefaults()
    {
        // Arrange & Act
        var venue = new Venue();

        // Assert
        Assert.Equal(0, venue.Id);
        Assert.Equal(string.Empty, venue.Name);
        Assert.Equal(string.Empty, venue.Address);
        Assert.Equal(0.0, venue.Latitude);
        Assert.Equal(0.0, venue.Longitude);
        Assert.NotNull(venue.Events);
        Assert.Empty(venue.Events);
    }

    [Fact]
    public void Venue_CanSetProperties()
    {
        // Arrange
        var venue = new Venue();

        // Act
        venue.Id = 1;
        venue.Name = "Test Venue";
        venue.Address = "123 Test St, Test City";
        venue.Latitude = 40.7589;
        venue.Longitude = -73.9851;
        venue.Description = "A test venue";

        // Assert
        Assert.Equal(1, venue.Id);
        Assert.Equal("Test Venue", venue.Name);
        Assert.Equal("123 Test St, Test City", venue.Address);
        Assert.Equal(40.7589, venue.Latitude);
        Assert.Equal(-73.9851, venue.Longitude);
        Assert.Equal("A test venue", venue.Description);
    }

    [Fact]
    public void Venue_CanAddEvents()
    {
        // Arrange
        var venue = new Venue { Id = 1, Name = "Test Venue" };
        var eventEntity = new Event { Id = 1, Title = "Test Event", VenueId = 1 };

        // Act
        venue.Events.Add(eventEntity);

        // Assert
        Assert.Single(venue.Events);
        Assert.Contains(eventEntity, venue.Events);
    }
}