using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries
{
    /// <summary>
    /// Represents a steelseries RGB-device.
    /// </summary>
    internal interface ISteelSeriesRGBDevice : IRGBDevice
    {
        void Initialize(UpdateQueue updateQueue, Dictionary<LedId, SteelSeriesLedId> ledMapping);
    }
}
