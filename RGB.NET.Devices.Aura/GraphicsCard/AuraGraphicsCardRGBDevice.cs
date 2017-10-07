using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Aura graphicsCard.
    /// </summary>
    public class AuraGraphicsCardRGBDevice : AuraRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AuraGraphicsCardRGBDevice"/>.
        /// </summary>
        public AuraGraphicsCardRGBDeviceInfo GraphicsCardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Aura.AuraGraphicsCardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Aura for the graphics card.</param>
        internal AuraGraphicsCardRGBDevice(AuraGraphicsCardRGBDeviceInfo info)
            : base(info)
        {
            this.GraphicsCardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AuraSDK.GetGPULedCount(GraphicsCardDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AuraLedId(this, AuraLedIds.GraphicsCardLed1 + i, i), new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 07.10.2017: We don'T know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Aura\GraphicsCards\{GraphicsCardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Aura\GraphicsCards"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AuraSDK.SetGPUColor(GraphicsCardDeviceInfo.Handle, ColorData);

        #endregion
    }
}
