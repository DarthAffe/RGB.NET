using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus mouse.
    /// </summary>
    public class AsusMouseRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the mouse.</param>
        internal AsusMouseRGBDevice(AsusRGBDeviceInfo info)
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
                InitializeLed(LedId.Mouse1 + i, new Rectangle(i * 10, 0, 10, 10));

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Mouses\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mouse1;

        #endregion
    }
}
