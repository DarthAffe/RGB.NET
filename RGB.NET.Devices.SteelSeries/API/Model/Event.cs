using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RGB.NET.Devices.SteelSeries.API.Model;

internal class Event
{
    #region Properties & Fields

    [JsonPropertyName("game")]
    public string? Game { get; set; }

    [JsonPropertyName("event")]
    public string? Name { get; set; }

    // ReSharper disable once CollectionNeverQueried.Global
    [JsonPropertyName("data")]
    public Dictionary<string, object> Data { get; } = new();

    #endregion

    #region Constructors

    public Event()
    { }

    public Event(Game game, string name)
    {
        this.Name = name;
        Game = game.Name;
    }

    #endregion
}