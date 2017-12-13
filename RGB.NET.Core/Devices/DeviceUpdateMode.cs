using System;

namespace RGB.NET.Core
{
    /// <summary>
    /// Contains a list of different device device update modes.
    /// </summary>
    [Flags]
    public enum DeviceUpdateMode
    {
        /// <summary>
        /// Represents nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents a mode which updates the leds of the device.
        /// </summary>
        Sync = 1 << 0,

        /// <summary>
        /// Represents a mode which reads the color of the leds of the device.
        /// This isn't supported by all devices!
        /// </summary>
        SyncBack = 1 << 1,

        /// <summary>
        /// Represents all update modes.
        /// </summary>
        NoUpdate = 1 << 0xFF
    }
}
