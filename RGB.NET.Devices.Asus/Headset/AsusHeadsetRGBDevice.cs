using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus headset.
    /// </summary>
    public class AsusHeadsetRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusHeadsetRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the headset.</param>
        internal AsusHeadsetRGBDevice(AsusRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = DeviceInfo.Device.Lights.Count;
            for (int i = 0; i < ledCount; i++)
                InitializeLed(LedId.Headset1 + i, new Rectangle(i * 40, 0, 40, 8));

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Headsets\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Headset1;

        #endregion
    }
}
