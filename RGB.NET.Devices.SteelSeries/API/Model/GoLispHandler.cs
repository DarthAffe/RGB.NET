using System.Text.Json.Serialization;

namespace RGB.NET.Devices.SteelSeries.API.Model;

internal class GoLispHandler
{
    #region Properties & Fields

    [JsonPropertyName("game")]
    public string? Game { get; set; }

    [JsonPropertyName("golisp")]
    public string? Handler { get; set; }

    #endregion

    #region Constructors

    public GoLispHandler()
    { }

    public GoLispHandler(Game game, string handler)
    {
        this.Handler = handler;
        Game = game.Name;
    }

    #endregion
}