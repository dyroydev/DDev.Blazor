using System.Globalization;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class OpenMeteoWeatherClient(IHttpClientFactory factory) : IWeatherClient
{
    public async Task<IReadOnlyList<DailyWeather>> GetDailyWeatherAsync(Location location)
    {
        using var client = factory.CreateClient("open-meteo");

        var response = await client.GetFromJsonAsync<JsonDocument>($"v1/forecast?latitude={location.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={location.Longitude.ToString(CultureInfo.InvariantCulture)}&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset&timezone=GMT");

        if (response is null)
            return [];

        var dates = response.RootElement.GetProperty("daily").GetProperty("time").EnumerateArray().Select(x => x.GetDateTime()).ToList();
        return dates.Select((date, index) => new DailyWeather
        {
            Location = location,
            Date = DateOnly.FromDateTime(date),
            Sunrise = TimeOnly.FromDateTime(response.RootElement.GetProperty("daily").GetProperty("sunrise").EnumerateArray().ElementAt(index).GetDateTime()),
            Sunset = TimeOnly.FromDateTime(response.RootElement.GetProperty("daily").GetProperty("sunset").EnumerateArray().ElementAt(index).GetDateTime()),
            MaxTemperature = response.RootElement.GetProperty("daily").GetProperty("temperature_2m_max").EnumerateArray().ElementAt(index).GetDouble(),
            MinTemperature = response.RootElement.GetProperty("daily").GetProperty("temperature_2m_min").EnumerateArray().ElementAt(index).GetDouble(),
            TemperatureUnit = response.RootElement.GetProperty("daily_units").GetProperty("temperature_2m_max").GetString() ?? "",
        }).ToList();
    }
}
