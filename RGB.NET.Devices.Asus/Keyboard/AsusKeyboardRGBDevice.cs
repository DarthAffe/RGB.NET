using RGB.NET.Core;
using RGB.NET.Devices.Asus.Native;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus keyboard.
    /// </summary>
    public class AsusKeyboardRGBDevice : AsusRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AsusKeyboardRGBDevice"/>.
        /// </summary>
        public AsusKeyboardRGBDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the keyboard.</param>
        internal AsusKeyboardRGBDevice(AsusKeyboardRGBDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AsusSDK.GetClaymoreKeyboardLedCount(KeyboardDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AsusLedId(this, AsusLedIds.KeyboardLed1 + i, i), new Rectangle(i * 19, 0, 19, 19));

            string model = KeyboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Keyboards\{model}\{KeyboardDeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                KeyboardDeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath(@"Images\Asus\Keyboards"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AsusSDK.SetClaymoreKeyboardColor(KeyboardDeviceInfo.Handle, ColorData);

        #endregion
    }
}
