// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a corsair headset.
    /// </summary>
    public class CorsairHeadsetRGBDevice : CorsairRGBDevice<CorsairHeadsetRGBDeviceInfo>, IHeadset
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset</param>
        internal CorsairHeadsetRGBDevice(CorsairHeadsetRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            AddLed(LedId.Headset1, new Point(0, 0), new Size(10, 10));
            AddLed(LedId.Headset2, new Point(10, 0), new Size(10, 10));
        }

        protected override object GetLedCustomData(LedId ledId) => HeadsetIdMapping.DEFAULT.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;

        #endregion
    }
}
