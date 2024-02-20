using System.Globalization;

namespace WeatherApp.Models;

public class Location
{
    public string? Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public string Url => $"/weather/{Latitude.ToString(CultureInfo.InvariantCulture)}/{Longitude.ToString(CultureInfo.InvariantCulture)}/{Name}";
}
