using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a MSI mainboard.
    /// </summary>
    public class MsiMainboardRGBDevice : MsiRGBDevice<MsiRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Msi.MsiMainboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by MSI for the mainboard.</param>
        internal MsiMainboardRGBDevice(MsiRGBDeviceInfo info)
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

                _MsiSDK.SetLedStyle(DeviceInfo.MsiDeviceType, i, LED_STYLE);
                InitializeLed(LedId.Mainboard1 + i, new Rectangle(i * 40, 0, 40, 8));
            }

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\MSI\Mainboards\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mainboard1;

        /// <inheritdoc />
        public override void SyncBack()
        { }

        #endregion
    }
}
