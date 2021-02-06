using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" />.
    /// </summary>
    public class AsusKeyboardRGBDeviceInfo : AsusRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public AsusPhysicalKeyboardLayout PhysicalLayout { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="device">The <see cref="IAuraSyncDevice"/> backing this RGB.NET device.</param>
        internal AsusKeyboardRGBDeviceInfo(IAuraSyncDevice device, AsusPhysicalKeyboardLayout layout)
            : base(RGBDeviceType.Keyboard, device, device.Name)
        {
            this.PhysicalLayout = layout;
        }

        #endregion
    }
}
