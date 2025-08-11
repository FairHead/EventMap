using EventMap.Client.Maui.Services;
using EventMap.Shared.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EventMap.Client.Maui.ViewModels;

// Simplified ViewModel for MVP skeleton
// In actual implementation, this would use CommunityToolkit.Mvvm and MAUI features
public class MapPageViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private bool _isLoading;
    private EventDto? _selectedEvent;
    private string _mapHtml = GenerateMapHtml();

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public EventDto? SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            _selectedEvent = value;
            OnPropertyChanged();
        }
    }

    public string MapHtml
    {
        get => _mapHtml;
        set
        {
            _mapHtml = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<EventDto> Events { get; } = new();

    public MapPageViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task LoadEventsAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Load events for New York area (sample bounding box)
            var request = new EventsQueryRequest
            {
                NorthEast_Lat = 40.800,
                NorthEast_Lng = -73.900,
                SouthWest_Lat = 40.700,
                SouthWest_Lng = -74.100
            };

            var events = await _apiService.GetEventsAsync(request);
            
            Events.Clear();
            foreach (var eventDto in events)
            {
                Events.Add(eventDto);
            }

            // Update map with new events
            MapHtml = GenerateMapHtml();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading events: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void SelectEvent(EventDto eventDto)
    {
        SelectedEvent = eventDto;
    }

    public void CloseEventDetails()
    {
        SelectedEvent = null;
    }

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static string GenerateMapHtml()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <title>EventMap - MVP Skeleton</title>
    <meta name='viewport' content='initial-scale=1,maximum-scale=1,user-scalable=no' />
    <script src='https://unpkg.com/maplibre-gl@latest/dist/maplibre-gl.js'></script>
    <link href='https://unpkg.com/maplibre-gl@latest/dist/maplibre-gl.css' rel='stylesheet' />
    <style>
        body { margin: 0; padding: 0; font-family: Arial, sans-serif; }
        #map { position: absolute; top: 0; bottom: 0; width: 100%; }
        .event-panel { 
            position: absolute; 
            left: 10px; 
            top: 10px; 
            width: 300px; 
            background: white; 
            padding: 20px; 
            border-radius: 8px; 
            box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
            display: none;
        }
        .marker { cursor: pointer; }
    </style>
</head>
<body>
    <div id='map'></div>
    <div id='event-panel' class='event-panel'>
        <h3 id='event-title'></h3>
        <p id='event-description'></p>
        <button onclick='closePanel()'>Close</button>
    </div>
    <script>
        var map = new maplibregl.Map({
            container: 'map',
            style: 'https://demotiles.maplibre.org/style.json',
            center: [-73.9857, 40.7484], // NYC
            zoom: 12
        });

        // Sample markers for MVP showing the concept
        var events = [
            { lng: -73.9654, lat: 40.7829, title: 'Jazz Night at Central Park', 
              description: 'Free outdoor jazz concert featuring local artists', id: 1 },
            { lng: -73.9776, lat: 40.7614, title: 'Street Art Festival', 
              description: 'Local street artists showcase their work', id: 2 },
            { lng: -73.9934, lat: 40.7505, title: 'Acoustic Coffee Session', 
              description: 'Intimate acoustic performances with coffee', id: 3 }
        ];

        events.forEach(function(event) {
            var el = document.createElement('div');
            el.className = 'marker';
            el.style.backgroundImage = 'url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTEyIDJDOC4xMzQgMiA1IDUuMTM0IDUgOUM1IDEwLjg4NSA1LjI5NSAxMi43MSA1Ljg0OCAxNC4zOTlMMTIgMjJMMTguMTUyIDE0LjM5OUMxOC43MDUgMTIuNzEgMTkgMTAuODg1IDE5IDlDMTkgNS4xMzQgMTUuODY2IDIgMTIgMlpNMTIgMTJDMTAuMzQzIDEyIDkgMTAuNjU3IDkgOUM5IDcuMzQzIDEwLjM0MyA2IDEyIDZDMTMuNjU3IDYgMTUgNy4zNDMgMTUgOUMxNSAxMC42NTcgMTMuNjU3IDEyIDEyIDEyWiIgZmlsbD0iIzUxMkJENCIvPgo8L3N2Zz4K)';
            el.style.width = '24px';
            el.style.height = '24px';
            el.style.backgroundSize = '100%';
            el.style.cursor = 'pointer';

            el.addEventListener('click', function() {
                showEventDetails(event);
            });

            new maplibregl.Marker(el)
                .setLngLat([event.lng, event.lat])
                .addTo(map);
        });

        function showEventDetails(event) {
            document.getElementById('event-title').textContent = event.title;
            document.getElementById('event-description').textContent = event.description;
            document.getElementById('event-panel').style.display = 'block';
        }

        function closePanel() {
            document.getElementById('event-panel').style.display = 'none';
        }

        // Add navigation controls
        map.addControl(new maplibregl.NavigationControl());
    </script>
</body>
</html>";
    }
}