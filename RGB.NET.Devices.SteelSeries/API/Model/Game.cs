using System.Text.Json.Serialization;

namespace RGB.NET.Devices.SteelSeries.API.Model;

internal class Game
{
    #region Properties & Fields

    [JsonPropertyName("game")]
    public string? Name { get; set; }

    [JsonPropertyName("game_display_name")]
    public string? DisplayName { get; set; }

    #endregion

    #region Constructors

    public Game()
    { }

    public Game(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    #endregion
}