using System.Text.Json.Serialization;

namespace RGB.NET.Devices.SteelSeries.API.Model;

internal sealed class CoreProps
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }
}