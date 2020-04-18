using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a Asus dram.
    /// </summary>
    public class AsusDramRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>, IDRAM
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusDramRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the DRAM.</param>
        internal AsusDramRGBDevice(AsusRGBDeviceInfo info)
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
                InitializeLed(LedId.DRAM1 + i, new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 21.10.2017: We don't know the model, how to save layouts and images?
            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, @"Layouts\Asus\Drams", $"{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.DRAM1;

        #endregion
    }
}
