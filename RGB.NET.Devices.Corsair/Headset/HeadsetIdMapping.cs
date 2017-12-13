using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    internal static class HeadsetIdMapping
    {
        internal static readonly Dictionary<LedId, CorsairLedId> DEFAULT = new Dictionary<LedId, CorsairLedId>
        {
            { LedId.Headset1, CorsairLedId.LeftLogo },
            { LedId.Headset2, CorsairLedId.RightLogo },
        };
    }
}
