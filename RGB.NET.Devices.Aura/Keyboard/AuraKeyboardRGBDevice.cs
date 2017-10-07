using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Aura keyboard.
    /// </summary>
    public class AuraKeyboardRGBDevice : AuraRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AuraKeyboardRGBDevice"/>.
        /// </summary>
        public AuraKeyboardRGBDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Aura.AuraKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Aura for the keyboard.</param>
        internal AuraKeyboardRGBDevice(AuraKeyboardRGBDeviceInfo info)
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
            int ledCount = _AuraSDK.GetClaymoreKeyboardLedCount(KeyboardDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AuraLedId(this, AuraLedIds.KeyboardLed1 + i, i), new Rectangle(i * 19, 0, 19, 19));

            string model = KeyboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Aura\Keyboards\{model}\{KeyboardDeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                KeyboardDeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath(@"Images\Aura\Keyboards"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AuraSDK.SetClaymoreKeyboardColor(KeyboardDeviceInfo.Handle, ColorData);

        #endregion
    }
}
