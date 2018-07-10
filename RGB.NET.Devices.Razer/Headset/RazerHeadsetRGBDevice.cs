// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a razer headset.
    /// </summary>
    public class RazerHeadsetRGBDevice : RazerRGBDevice<RazerHeadsetRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerHeadsetRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset.</param>
        internal RazerHeadsetRGBDevice(RazerHeadsetRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Razer\Headset\{model}.xml"), null);

            if (LedMapping.Count == 0)
                for (int i = 0; i < _Defines.HEADSET_MAX_LEDS; i++)
                    InitializeLed(LedId.Headset1 + i, new Rectangle(i * 11, 0, 10, 10));
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Headset1;

        /// <inheritdoc />
        protected override RazerUpdateQueue CreateUpdateQueue(IDeviceUpdateTrigger updateTrigger) => new RazerHeadsetUpdateQueue(updateTrigger, DeviceInfo.RazerDeviceId);

        #endregion
    }
}
