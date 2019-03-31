using Newtonsoft.Json;

namespace RGB.NET.Devices.SteelSeries.API.Model
{
    internal class Game
    {
        #region Properties & Fields

        [JsonProperty("game")]
        public string Name { get; set; }

        [JsonProperty("game_display_name")]
        public string DisplayName { get; set; }

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
}
