// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.IO;
using System.Reflection;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a logitech keyboard.
    /// </summary>
    public class LogitechKeyboardRGBDevice : LogitechRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="LogitechKeyboardRGBDevice"/>.
        /// </summary>
        public LogitechKeyboardRGBDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechKeyboardRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard</param>
        internal LogitechKeyboardRGBDevice(LogitechKeyboardRGBDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            ApplyLayoutFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                $@"Layouts\Logitech\Keyboards\{KeyboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}\{KeyboardDeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"));
        }

        #endregion
    }
}
