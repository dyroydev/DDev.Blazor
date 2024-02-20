using System.Text.Json.Serialization;

namespace WeatherApp.Models;

public class WeatherDto
{
    [JsonPropertyName("daily_units.temperature_2m_max")]
    public string TemperatureUnit { get; init; } = default!;

    [JsonPropertyName("daily.time")]
    public IReadOnlyList<DateOnly> Dates { get; init; } = [];

    [JsonPropertyName("daily.temperature_2m_max")]
    public IReadOnlyList<double> MaxTemperatures { get; init; } = [];

    [JsonPropertyName("daily.temperature_2m_max")]
    public IReadOnlyList<double> MinTemperatures { get; init; } = [];

    [JsonPropertyName("daily.sunrise")]
    public IReadOnlyList<TimeOnly> Sunrises { get; init; } = [];

    [JsonPropertyName("daily.sunset")]
    public IReadOnlyList<TimeOnly> Sunsets { get; init; } = [];
}

public class DailyWeather
{
    public required Location Location { get; init; }

    public DateOnly Date { get; init; }

    public TimeOnly Sunrise { get; init; }

    public TimeOnly Sunset { get; init; }

    public double MaxTemperature { get; init; }

    public double MinTemperature { get; init; }

    public required string TemperatureUnit { get; init; }
}
