using System;

namespace RGB.NET.Core
{
    [Flags]
    public enum DeviceUpdateMode
    {
        None = 0,

        Sync = 1 << 0,
        SyncBack = 1 << 1,

        NoUpdate = 1 << 0xFF
    }
}
