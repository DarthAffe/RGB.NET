using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Aura mainboard.
    /// </summary>
    public class AuraMainboardRGBDevice : AuraRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="AuraMainboardRGBDevice"/>.
        /// </summary>
        public AuraMainboardRGBDeviceInfo MainboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Aura.AuraMainboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Aura for the mainboard.</param>
        internal AuraMainboardRGBDevice(AuraMainboardRGBDeviceInfo info)
            : base(info)
        {
            this.MainboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 07.10.2017: Look for a good default layout
            int ledCount = _AuraSDK.GetMbLedCount(MainboardDeviceInfo.Handle);
            for (int i = 0; i < ledCount; i++)
                InitializeLed(new AuraLedId(this, AuraLedIds.MainboardAudio1 + i, i), new Rectangle(i * 40, 0, 40, 8));

            //TODO DarthAffe 07.10.2017: We don'T know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Aura\Mainboards\{MainboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Aura\Mainboards"));
        }

        /// <inheritdoc />
        protected override void ApplyColorData() => _AuraSDK.SetMbColor(MainboardDeviceInfo.Handle, ColorData);

        #endregion
    }
}
