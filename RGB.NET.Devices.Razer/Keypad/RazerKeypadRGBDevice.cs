// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="RazerRGBDevice" />
    /// <summary>
    /// Represents a razer keypad.
    /// </summary>
    public class RazerKeypadRGBDevice : RazerRGBDevice, IKeypad
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeypadRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keypad.</param>
        internal RazerKeypadRGBDevice(RazerRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            for (int i = 0; i < _Defines.KEYPAD_MAX_ROW; i++)
                for (int j = 0; j < _Defines.KEYPAD_MAX_COLUMN; j++)
                    AddLed(LedId.Keypad1 + ((i * _Defines.KEYPAD_MAX_COLUMN) + j), new Point(j * 20, i * 20), new Size(19, 19));
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Keypad1;

        /// <inheritdoc />
        protected override RazerUpdateQueue CreateUpdateQueue(IDeviceUpdateTrigger updateTrigger) => new RazerKeypadUpdateQueue(updateTrigger);

        #endregion
    }
}
