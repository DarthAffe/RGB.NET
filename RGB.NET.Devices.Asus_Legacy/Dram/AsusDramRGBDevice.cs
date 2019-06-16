//using RGB.NET.Core;
//using RGB.NET.Devices.Asus.Native;

//namespace RGB.NET.Devices.Asus
//{
//    /// <inheritdoc />
//    /// <summary>
//    /// Represents a Asus dram.
//    /// </summary>
//    public class AsusDramRGBDevice : AsusRGBDevice
//    {
//        #region Properties & Fields

//        /// <summary>
//        /// Gets information about the <see cref="AsusDramRGBDevice"/>.
//        /// </summary>
//        public AsusDramRGBDeviceInfo DramDeviceInfo { get; }

//        #endregion

//        #region Constructors

//        /// <inheritdoc />
//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusDramRGBDevice" /> class.
//        /// </summary>
//        /// <param name="info">The specific information provided by Asus for the DRAM.</param>
//        internal AsusDramRGBDevice(AsusDramRGBDeviceInfo info)
//            : base(info)
//        {
//            this.DramDeviceInfo = info;
//        }

//        #endregion

//        #region Methods

//        /// <inheritdoc />
//        protected override void InitializeLayout()
//        {
//            //TODO DarthAffe 21.10.2017: Look for a good default layout
//            int ledCount = _AsusSDK.GetGPULedCount(DramDeviceInfo.Handle);
//            for (int i = 0; i < ledCount; i++)
//                InitializeLed(new AsusLedId(this, AsusLedIds.DramLed1 + i, i), new Rectangle(i * 10, 0, 10, 10));

//            //TODO DarthAffe 21.10.2017: We don't know the model, how to save layouts and images?
//            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Drams\{DramDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
//                null, PathHelper.GetAbsolutePath(@"Images\Asus\Drams"));
//        }

//        /// <inheritdoc />
//        protected override void ApplyColorData() => _AsusSDK.SetDramColor(DramDeviceInfo.Handle, ColorData);

//        #endregion
//    }
//}
