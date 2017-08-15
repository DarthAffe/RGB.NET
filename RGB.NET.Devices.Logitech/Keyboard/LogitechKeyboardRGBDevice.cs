// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

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
        /// <param name="info">The specific information provided by logitech for the keyboard</param>
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
            string model = KeyboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\Logitech\Keyboards\{model}\{KeyboardDeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                KeyboardDeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath($@"Images\Logitech\Keyboards"));
        }

        #endregion
    }
}
