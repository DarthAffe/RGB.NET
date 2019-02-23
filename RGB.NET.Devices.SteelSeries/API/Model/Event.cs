using System.Collections.Generic;
using Newtonsoft.Json;

namespace RGB.NET.Devices.SteelSeries.API.Model
{
    internal class Event
    {
        #region Properties & Fields

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("event")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; } = new Dictionary<string, object>();

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
}
