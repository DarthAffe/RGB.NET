using RGB.NET.Core;
using RGB.NET.Devices.Asus.Native;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus mouse.
    /// </summary>
    public class AsusMouseRGBDevice : AsusRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AsusMouseRGBDevice"/>.
        /// </summary>
        public AsusMouseRGBDeviceInfo MouseDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the mouse.</param>
        internal AsusMouseRGBDevice(AsusMouseRGBDeviceInfo info)
            : base(info)
        {
            this.MouseDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AsusSDK.GetRogMouseLedCount(MouseDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AsusLedId(this, AsusLedIds.MouseLed1 + i, i), new Rectangle(i * 10, 0, 10, 10));
            
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Mouses\{MouseDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Asus\Mouses"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AsusSDK.SetRogMouseColor(MouseDeviceInfo.Handle, ColorData);

        #endregion
    }
}
