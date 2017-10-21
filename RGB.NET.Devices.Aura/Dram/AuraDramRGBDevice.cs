using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Aura dram.
    /// </summary>
    public class AuraDramRGBDevice : AuraRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AuraDramRGBDevice"/>.
        /// </summary>
        public AuraDramRGBDeviceInfo DramDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Aura.AuraDramRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Aura for the DRAM.</param>
        internal AuraDramRGBDevice(AuraDramRGBDeviceInfo info)
            : base(info)
        {
            this.DramDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 21.10.2017: Look for a good default layout
            int ledCount = _AuraSDK.GetGPULedCount(DramDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AuraLedId(this, AuraLedIds.DramLed1 + i, i), new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 21.10.2017: We don'T know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Aura\Drams\{DramDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Aura\Drams"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AuraSDK.SetDramColor(DramDeviceInfo.Handle, ColorData);

        #endregion
    }
}
