// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a razer mousepad.
    /// </summary>
    public class RazerMousepadRGBDevice : RazerRGBDevice<RazerMousepadRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerMousepadRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mousepad.</param>
        internal RazerMousepadRGBDevice(RazerMousepadRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Razer\Mousepad\{model}.xml"), null);

            if (LedMapping.Count == 0)
                for (int i = 0; i < _Defines.MOUSEPAD_MAX_LEDS; i++)
                    InitializeLed(LedId.Mousepad1 + i, new Rectangle(i * 11, 0, 10, 10));
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mousepad1;

        /// <inheritdoc />
        protected override RazerUpdateQueue CreateUpdateQueue(IDeviceUpdateTrigger updateTrigger) => new RazerMousepadUpdateQueue(updateTrigger, DeviceInfo.RazerDeviceId);

        #endregion
    }
}
