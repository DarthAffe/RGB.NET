using RGB.NET.Core.Layout;

namespace RGB.NET.Core
{
    /// <summary>
    /// Contains a list of different lightning-modes used by <see cref="DeviceLayout"/>.
    /// </summary>
    public enum RGBDeviceLighting
    {
        /// <summary>
        /// The <see cref="IRGBDevice"/> doesn't support lighting,
        /// </summary>
        None = 0,

        /// <summary>
        /// The <see cref="IRGBDevice"/> supports per-key-lightning.
        /// </summary>
        Key = 1,

        /// <summary>
        /// The <see cref="IRGBDevice"/> supports per-device-lightning.
        /// </summary>
        Device = 2,
    }
}
