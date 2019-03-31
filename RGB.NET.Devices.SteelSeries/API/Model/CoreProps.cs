using Newtonsoft.Json;

namespace RGB.NET.Devices.SteelSeries.API.Model
{
    internal class CoreProps
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}
