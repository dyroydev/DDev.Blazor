using WeatherApp.Models;

namespace WeatherApp.Services;

public interface IWeatherClient
{
    Task<IReadOnlyList<DailyWeather>> GetDailyWeatherAsync(Location location);
}
