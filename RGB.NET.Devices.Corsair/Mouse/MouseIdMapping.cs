using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    internal static class MouseIdMapping
    {
        internal static readonly Dictionary<LedId, CorsairLedId> DEFAULT = new Dictionary<LedId, CorsairLedId>
        {
            { LedId.Mouse1, CorsairLedId.B1 },
            { LedId.Mouse2, CorsairLedId.B2 },
            { LedId.Mouse3, CorsairLedId.B3 },
            { LedId.Mouse4, CorsairLedId.B4 },
            { LedId.Mouse5, CorsairLedId.B5 },
            { LedId.Mouse6, CorsairLedId.B6 },
        };

        internal static readonly Dictionary<LedId, CorsairLedId> GLAIVE = new Dictionary<LedId, CorsairLedId>
        {
            { LedId.Mouse1, CorsairLedId.B1 },
            { LedId.Mouse2, CorsairLedId.B2 },
            { LedId.Mouse3, CorsairLedId.B5 },
        };

        internal static readonly Dictionary<LedId, CorsairLedId> M65_RGB_ELITE = new Dictionary<LedId, CorsairLedId>
        {
            { LedId.Mouse1, CorsairLedId.B1 },
            { LedId.Mouse2, CorsairLedId.B3 },
        };
    }
}
