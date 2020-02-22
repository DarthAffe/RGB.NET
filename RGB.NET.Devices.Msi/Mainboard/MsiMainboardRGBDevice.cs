using System;
using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;
using RGB.NET.Devices.Msi.Exceptions;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Msi mainboard.
    /// </summary>
    public class MsiMainboardRGBDevice : MsiRGBDevice<MsiRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Msi.MsiMainboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the mainboard.</param>
        internal MsiMainboardRGBDevice(MsiRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
           // Should errors be handled?
            _MsiSDK.GetDeviceInfo(out string[] deviceTypes, out int[] ledCounts);

            for (int i = 0; i < deviceTypes.Length; i++)
            {
                // DeviceInfo.MsiDeviceType = "MSI_MB"
                if (deviceTypes[i].Equals(DeviceInfo.MsiDeviceType))
                {
                    for (int j = 0; j < ledCounts[i]; j++)
                    {
                        // Should it be configurable in order to provide style access?
                        // Sets led style to "Steady" in order to have a solid color output therefore a controllable led color
                        // This is a string defined by the output of _MsiSDK.GetLedStyle, "Steady" should be always present
                        string style = "Steady";

                        _MsiSDK.SetLedStyle(DeviceInfo.MsiDeviceType, j, style);
                        InitializeLed(LedId.Mainboard1 + j, new Rectangle(j * 40, 0, 40, 8));
                    }
                }
            }

            //TODO DarthAffe 07.10.2017: We don't know the model, how to save layouts and images?
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Asus\Mainboards\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mainboard1;

        /// <inheritdoc />
        public override void SyncBack() 
        { }

        /// <inheritdoc />
        
        #endregion
    }
}
