using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Aura mouse.
    /// </summary>
    public class AuraMouseRGBDevice : AuraRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AuraMouseRGBDevice"/>.
        /// </summary>
        public AuraMouseRGBDeviceInfo MouseDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Aura.AuraMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Aura for the mouse.</param>
        internal AuraMouseRGBDevice(AuraMouseRGBDeviceInfo info)
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
            int ledCount = _AuraSDK.GetRogMouseLedCount(MouseDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AuraLedId(this, AuraLedIds.MouseLed1 + i, i), new Rectangle(i * 10, 0, 10, 10));
            
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Aura\Mouses\{MouseDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Aura\Mouses"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AuraSDK.SetRogMouseColor(MouseDeviceInfo.Handle, ColorData);

        #endregion
    }
}
