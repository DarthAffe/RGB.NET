using System;
using RGB.NET.Core;
using RGB.NET.Devices.Asus.Native;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus graphicsCard.
    /// </summary>
    public class AsusGraphicsCardRGBDevice : AsusRGBDevice<AsusGraphicsCardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusGraphicsCardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the graphics card.</param>
        internal AsusGraphicsCardRGBDevice(AsusGraphicsCardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AsusSDK.GetGPULedCount(DeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(LedId.GraphicsCard1 + i, new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\GraphicsCards\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.GraphicsCard1;

        /// <inheritdoc />
        protected override Action<IntPtr, byte[]> GetUpdateColorAction() => _AsusSDK.SetGPUColor;

        #endregion
    }
}
