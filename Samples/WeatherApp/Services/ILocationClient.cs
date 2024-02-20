using WeatherApp.Models;

namespace WeatherApp.Services;

public interface ILocationClient
{
    Task<List<Location>> SearchCitiesAsync(string name);
}
