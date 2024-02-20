using DDev.Blazor.Services;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class FavoriteLocationsService(ILocalStorage storage)
{
    public event Action? Changed;

    public async Task AddFavorite(Location location)
    {
        var favorites = await GetFavoriteLocations();
        await SetFavoriteLocations([..favorites, location]);
        Changed?.Invoke();
    }

    public async Task RemoveFavorite(Location location)
    {
        var favorites = await GetFavoriteLocations();
        favorites = favorites.Where(f => f.Name != location.Name).ToList();
        await SetFavoriteLocations(favorites);
        Changed?.Invoke();
    }

    public async Task<bool> IsFavorite(Location location)
    {
        var favorites = await GetFavoriteLocations();
        return favorites.Any(f => f.Name == location.Name);
    }

    public async Task<IReadOnlyList<Location>> GetFavoriteLocations()
    {
        var json = await storage.GetAsync("favoriteLocations");
        try
        {
            return JsonSerializer.Deserialize<List<Location>>(json!) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private async Task SetFavoriteLocations(IReadOnlyList<Location> locations)
    {
        var json = JsonSerializer.Serialize(locations);
        await storage.SetAsync("favoriteLocations", json);
    }
}
