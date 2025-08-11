using EventMap.Shared.DTOs;
using System.Text.Json;

namespace EventMap.Client.Maui.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<List<EventDto>> GetEventsAsync(EventsQueryRequest request)
    {
        try
        {
            var queryParams = new List<string>();
            
            if (request.NorthEast_Lat.HasValue)
                queryParams.Add($"northEast_Lat={request.NorthEast_Lat}");
            if (request.NorthEast_Lng.HasValue)
                queryParams.Add($"northEast_Lng={request.NorthEast_Lng}");
            if (request.SouthWest_Lat.HasValue)
                queryParams.Add($"southWest_Lat={request.SouthWest_Lat}");
            if (request.SouthWest_Lng.HasValue)
                queryParams.Add($"southWest_Lng={request.SouthWest_Lng}");
            
            if (request.Genres?.Length > 0)
            {
                foreach (var genre in request.Genres)
                    queryParams.Add($"genres={Uri.EscapeDataString(genre)}");
            }
            
            if (request.StartAfter.HasValue)
                queryParams.Add($"startAfter={request.StartAfter:O}");
            if (request.StartBefore.HasValue)
                queryParams.Add($"startBefore={request.StartBefore:O}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"events{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EventDto>>(json, _jsonOptions) ?? new List<EventDto>();
            }
            
            return new List<EventDto>();
        }
        catch (Exception ex)
        {
            // Log error in production
            System.Diagnostics.Debug.WriteLine($"Error fetching events: {ex.Message}");
            return new List<EventDto>();
        }
    }

    public async Task<EventDto?> GetEventAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"events/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EventDto>(json, _jsonOptions);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching event {id}: {ex.Message}");
            return null;
        }
    }
}