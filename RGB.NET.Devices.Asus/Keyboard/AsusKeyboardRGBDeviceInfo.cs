using System.Collections.Generic;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" />.
    /// </summary>
    public class AsusKeyboardRGBDeviceInfo : AsusRGBDeviceInfo, IKeyboardDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// The ASUS SDK returns useless names for notebook keyboards, possibly for others as well.
        /// Keep a list of those and rely on <see cref="WMIHelper.GetSystemModelInfo()"/> to get the real model
        /// </summary>
        private static List<string> GenericDeviceNames = new() {"NotebookKeyboard"};

        /// <inheritdoc />
        public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="device">The <see cref="IAuraSyncDevice"/> backing this RGB.NET device.</param>
        internal AsusKeyboardRGBDeviceInfo(IAuraSyncDevice device)
            : base(RGBDeviceType.Keyboard, device, GetKeyboardModel(device.Name))
        { }

        #endregion

        #region Methods

        private static string? GetKeyboardModel(string deviceName)
        {
            return GenericDeviceNames.Contains(deviceName) ? WMIHelper.GetSystemModelInfo() : deviceName;
        }

        #endregion
    }
}
