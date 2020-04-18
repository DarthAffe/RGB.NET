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
            InitializeLed(LedId.Headset1, new Rectangle(0, 0, 10, 10));
            InitializeLed(LedId.Headset2, new Rectangle(10, 0, 10, 10));

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, @"Layouts\Corsair\Headsets", $"{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => HeadsetIdMapping.DEFAULT.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;

        #endregion
    }
}
