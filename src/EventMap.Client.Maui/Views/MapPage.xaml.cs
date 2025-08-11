using EventMap.Client.Maui.ViewModels;

namespace EventMap.Client.Maui.Views;

// Simplified MapPage for MVP skeleton
// In actual MAUI implementation, this would inherit from ContentPage
public class MapPage
{
    public MapPageViewModel ViewModel { get; }

    public MapPage(MapPageViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public async Task OnAppearing()
    {
        await ViewModel.LoadEventsAsync();
    }
}