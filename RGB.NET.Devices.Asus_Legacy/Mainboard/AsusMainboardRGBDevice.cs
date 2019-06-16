using System;
using RGB.NET.Core;
using RGB.NET.Devices.Asus.Native;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus mainboard.
    /// </summary>
    public class AsusMainboardRGBDevice : AsusRGBDevice<AsusMainboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusMainboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the mainboard.</param>
        internal AsusMainboardRGBDevice(AsusMainboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AsusSDK.GetMbLedCount(DeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(LedId.Mainboard1 + i, new Rectangle(i * 40, 0, 40, 8));

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Mainboards\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mainboard1;

        /// <inheritdoc />
        public override void SyncBack()
        {
            byte[] colorData = _AsusSDK.GetMbColor(DeviceInfo.Handle);
            for (int i = 0; i < LedMapping.Count; i++)
                SetLedColorWithoutRequest(LedMapping[LedId.Mainboard1 + i], new Color(colorData[(i * 3)], colorData[(i * 3) + 2], colorData[(i * 3) + 1]));
        }

        /// <inheritdoc />
        protected override Action<IntPtr, byte[]> GetUpdateColorAction() => _AsusSDK.SetMbColor;

        #endregion
    }
}
