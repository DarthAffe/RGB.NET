using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    internal static class MemoryIdMapping
    {
        internal static readonly Dictionary<LedId, CorsairLedId> DEFAULT = new Dictionary<LedId, CorsairLedId>
        {
            { LedId.Invalid, CorsairLedId.Invalid },
            { LedId.DRAM1, CorsairLedId.DRAM1 },
            { LedId.DRAM2, CorsairLedId.DRAM2 },
            { LedId.DRAM3, CorsairLedId.DRAM3 },
            { LedId.DRAM4, CorsairLedId.DRAM4 },
            { LedId.DRAM5, CorsairLedId.DRAM5 },
            { LedId.DRAM6, CorsairLedId.DRAM6 },
            { LedId.DRAM7, CorsairLedId.DRAM7 },
            { LedId.DRAM8, CorsairLedId.DRAM8 },
            { LedId.DRAM9, CorsairLedId.DRAM9 },
            { LedId.DRAM10, CorsairLedId.DRAM10 },
            { LedId.DRAM11, CorsairLedId.DRAM11 },
            { LedId.DRAM12, CorsairLedId.DRAM12 },
        };
    }
}
