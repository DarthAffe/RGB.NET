// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="RazerRGBDevice" />
    /// <summary>
    /// Represents a razer mouse.
    /// </summary>
    public class RazerMouseRGBDevice : RazerRGBDevice, IMouse
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mouse.</param>
        internal RazerMouseRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
            : base(info, new RazerMouseUpdateQueue(updateTrigger))
        {
            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            for (int i = 0; i < _Defines.MOUSE_MAX_ROW; i++)
                for (int j = 0; j < _Defines.MOUSE_MAX_COLUMN; j++)
                    AddLed(LedId.Mouse1 + ((i * _Defines.MOUSE_MAX_COLUMN) + j), new Point(j * 11, i * 11), new Size(10, 10));
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mouse1;

        #endregion
    }
}
