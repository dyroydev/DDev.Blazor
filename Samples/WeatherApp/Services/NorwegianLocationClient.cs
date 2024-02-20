using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class NorwegianLocationClient(IHttpClientFactory factory) : ILocationClient
{
    public async Task<List<Location>> SearchCitiesAsync(string name)
    {
        using var client = factory.CreateClient("geonorge");

        name = Uri.EscapeDataString(name + '*');

        var response = await client.GetFromJsonAsync<JsonDocument>($"/stedsnavn/v1/navn?sok={name}&utkoordsystem=4326&treffPerSide=10&side=1");

        if (response is null)
            return [];

        return response.RootElement.GetProperty("navn").EnumerateArray().Where(x => x.GetProperty("navneobjekttype").GetString() == "By").Select(x => new Location
        {
            Name = x.GetProperty("skrivemåte").GetString() ?? "",
            Latitude = x.GetProperty("representasjonspunkt").GetProperty("nord").GetDouble(),
            Longitude = x.GetProperty("representasjonspunkt").GetProperty("øst").GetDouble(),
        }).ToList();
    }

}
