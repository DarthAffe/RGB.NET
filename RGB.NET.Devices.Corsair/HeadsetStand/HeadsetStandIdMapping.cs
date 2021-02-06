using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    internal static class HeadsetStandIdMapping
    {
        internal static readonly Dictionary<LedId, CorsairLedId> DEFAULT = new()
                                                                           {
            { LedId.HeadsetStand1, CorsairLedId.HeadsetStandZone1 },
            { LedId.HeadsetStand2, CorsairLedId.HeadsetStandZone2 },
            { LedId.HeadsetStand3, CorsairLedId.HeadsetStandZone3 },
            { LedId.HeadsetStand4, CorsairLedId.HeadsetStandZone4 },
            { LedId.HeadsetStand5, CorsairLedId.HeadsetStandZone5 },
            { LedId.HeadsetStand6, CorsairLedId.HeadsetStandZone6 },
            { LedId.HeadsetStand7, CorsairLedId.HeadsetStandZone7 },
            { LedId.HeadsetStand8, CorsairLedId.HeadsetStandZone8 },
            { LedId.HeadsetStand9, CorsairLedId.HeadsetStandZone9 }
        };
    }
}
