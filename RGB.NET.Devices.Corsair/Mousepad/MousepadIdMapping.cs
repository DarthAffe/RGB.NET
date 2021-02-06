using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    internal static class MousepadIdMapping
    {
        internal static readonly Dictionary<LedId, CorsairLedId> DEFAULT = new()
                                                                           {
            { LedId.Mousepad1, CorsairLedId.Zone1 },
            { LedId.Mousepad2, CorsairLedId.Zone2 },
            { LedId.Mousepad3, CorsairLedId.Zone3 },
            { LedId.Mousepad4, CorsairLedId.Zone4 },
            { LedId.Mousepad5, CorsairLedId.Zone5 },
            { LedId.Mousepad6, CorsairLedId.Zone6 },
            { LedId.Mousepad7, CorsairLedId.Zone7 },
            { LedId.Mousepad8, CorsairLedId.Zone8 },
            { LedId.Mousepad9, CorsairLedId.Zone9 },
            { LedId.Mousepad10, CorsairLedId.Zone10 },
            { LedId.Mousepad11, CorsairLedId.Zone11 },
            { LedId.Mousepad12, CorsairLedId.Zone12 },
            { LedId.Mousepad13, CorsairLedId.Zone13 },
            { LedId.Mousepad14, CorsairLedId.Zone14 },
            { LedId.Mousepad15, CorsairLedId.Zone15 }
        };
    }
}
