// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a corsair headset.
    /// </summary>
    public class CorsairHeadsetRGBDevice : CorsairRGBDevice<CorsairHeadsetRGBDeviceInfo>
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
            InitializeLed(new CorsairLedId(this, CorsairLedIds.LeftLogo), new Rectangle(0, 0, 10, 10));
            InitializeLed(new CorsairLedId(this, CorsairLedIds.RightLogo), new Rectangle(10, 0, 10, 10));

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Corsair\Headsets\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Corsair\Headsets"));
        }

        #endregion
    }
}
