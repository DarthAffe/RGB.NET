// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="RazerRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a razer chroma link.
    /// </summary>
    public class RazerChromaLinkRGBDevice : RazerRGBDevice<RazerChromaLinkRGBDeviceInfo>, ILedStripe
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerChromaLinkRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the chroma link.</param>
        internal RazerChromaLinkRGBDevice(RazerChromaLinkRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, @"Layouts\Razer\ChromaLink", $"{model}.xml"), null);

            if (LedMapping.Count == 0)
                for (int i = 0; i < _Defines.CHROMALINK_MAX_LEDS; i++)
                    InitializeLed(LedId.Custom1 + i, new Rectangle(i * 11, 0, 10, 10));
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Custom1;

        /// <inheritdoc />
        protected override RazerUpdateQueue CreateUpdateQueue(IDeviceUpdateTrigger updateTrigger) => new RazerChromaLinkUpdateQueue(updateTrigger, DeviceInfo.DeviceId);

        #endregion
    }
}
