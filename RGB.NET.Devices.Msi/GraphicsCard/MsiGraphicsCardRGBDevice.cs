using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc />
    /// <summary>
    /// Represents MSI VGA adapters.
    /// </summary>
    public class MsiGraphicsCardRGBDevice : MsiRGBDevice<MsiRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Msi.MsiGraphicsCardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by MSI for graphics cards.</param>
        internal MsiGraphicsCardRGBDevice(MsiRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout(int ledCount)
        {
            for (int i = 0; i < ledCount; i++)
            {
                //Hex3l: Should it be configurable in order to provide style access?
                //Hex3l: Sets led style to "Steady" in order to have a solid color output therefore a controllable led color
                //Hex3l: This is a string defined by the output of _MsiSDK.GetLedStyle, "Steady" should be always present
                const string LED_STYLE = "Steady";

                //Hex3l: Every led is a video card adapter.

                _MsiSDK.SetLedStyle(DeviceInfo.MsiDeviceType, i, LED_STYLE);
                InitializeLed(LedId.GraphicsCard1 + i, new Rectangle(i * 10, 0, 10, 10));
            }

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\MSI\GraphicsCard\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.GraphicsCard1;

        /// <inheritdoc />
        public override void SyncBack()
        { }

        #endregion
    }
}
