// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="RazerRGBDevice" />
    /// <summary>
    /// Represents a razer keyboard.
    /// </summary>
    public class RazerKeyboardRGBDevice : RazerRGBDevice, IKeyboard
    {
        #region Properties & Fields

        IKeyboardDeviceInfo IKeyboard.DeviceInfo => (IKeyboardDeviceInfo) DeviceInfo;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard.</param>
        internal RazerKeyboardRGBDevice(RazerKeyboardRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
            : base(info, new RazerKeyboardUpdateQueue(updateTrigger))
        {
            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            for (int i = 0; i < _Defines.KEYBOARD_MAX_ROW; i++)
                for (int j = 0; j < _Defines.KEYBOARD_MAX_COLUMN; j++)
                    AddLed(LedId.Keyboard_Escape + ((i * _Defines.KEYBOARD_MAX_COLUMN) + j), new Point(j * 20, i * 20), new Size(19, 19));
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Keyboard_Escape;

        #endregion
    }
}
