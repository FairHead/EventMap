using EventMap.Client.Maui.Services;
using EventMap.Client.Maui.ViewModels;
using EventMap.Shared.DTOs;

namespace EventMap.Client.Maui;

// MVP Skeleton Demo Program
// This demonstrates the MAUI client structure without full MAUI dependencies
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🗺️ EventMap MVP Skeleton - MAUI Client Demo");
        Console.WriteLine("==============================================");

        // Create HTTP client for API communication
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5032/api/");

        // Create API service
        var apiService = new ApiService(httpClient);

        // Create ViewModel (as would be used in MAUI)
        var viewModel = new MapPageViewModel(apiService);

        Console.WriteLine("\n📍 Loading events from API...");

        try
        {
            // Load events (this simulates what would happen in MAUI OnAppearing)
            await viewModel.LoadEventsAsync();

            Console.WriteLine($"✅ Loaded {viewModel.Events.Count} events:");

            foreach (var eventDto in viewModel.Events)
            {
                Console.WriteLine($"   • {eventDto.Title} ({eventDto.Latitude}, {eventDto.Longitude})");
                Console.WriteLine($"     {eventDto.Description}");
                Console.WriteLine($"     Genres: {string.Join(", ", eventDto.Genres)}");
                if (eventDto.Venue != null)
                {
                    Console.WriteLine($"     Venue: {eventDto.Venue.Name} - {eventDto.Venue.Address}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n🗺️ MapLibre GL Map HTML Generated:");
            Console.WriteLine("(This would be displayed in MAUI WebView)");
            Console.WriteLine("Length: " + viewModel.MapHtml.Length + " characters");

            // Simulate selecting an event
            if (viewModel.Events.Count > 0)
            {
                var firstEvent = viewModel.Events.First();
                viewModel.SelectEvent(firstEvent);
                Console.WriteLine($"\n🎯 Selected Event: {viewModel.SelectedEvent?.Title}");
                Console.WriteLine("(This would show the event details panel in MAUI)");
            }

            Console.WriteLine("\n✨ MVP Skeleton Demo Complete!");
            Console.WriteLine("In the full MAUI implementation:");
            Console.WriteLine("- This would run on Android/iOS devices");
            Console.WriteLine("- MapLibre GL would show interactive map with pins");
            Console.WriteLine("- Tapping pins would show slide-in event details panel");
            Console.WriteLine("- SignalR would provide real-time event updates");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine("Make sure the API server is running on localhost:5032");
        }
    }
}