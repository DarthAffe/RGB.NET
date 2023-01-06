using System.Text.Json.Serialization;

namespace RGB.NET.Devices.SteelSeries.API.Model;

internal class CoreProps
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }
}