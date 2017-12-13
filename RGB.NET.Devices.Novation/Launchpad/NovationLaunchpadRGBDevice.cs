using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Novation launchpad.
    /// </summary>
    public class NovationLaunchpadRGBDevice : NovationRGBDevice<NovationLaunchpadRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.NovationLaunchpadRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Novation for the launchpad</param>
        internal NovationLaunchpadRGBDevice(NovationLaunchpadRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<NovationLedId, LedId> mapping = LaunchpadIdMapping.DEFAULT.SwapKeyValue();

            //TODO DarthAffe 15.08.2017: Check if all launchpads are using the same basic layout
            const int BUTTON_SIZE = 20;
            foreach (NovationLedId ledId in Enum.GetValues(typeof(NovationLedId)))
            {
                Rectangle rectangle;
                if (ledId.IsCustom())
                    rectangle = new Rectangle(BUTTON_SIZE * (ledId.GetId() - 0x68), 0, BUTTON_SIZE, BUTTON_SIZE);
                else if (ledId.IsScene())
                    rectangle = new Rectangle(8 * BUTTON_SIZE, BUTTON_SIZE * (((int)ledId.GetId() / 0x10) + 1), BUTTON_SIZE, BUTTON_SIZE);
                else if (ledId.IsGrid())
                    rectangle = new Rectangle(BUTTON_SIZE * ((int)ledId.GetId() % 0x10), BUTTON_SIZE * (((int)ledId.GetId() / 0x10) + 1), BUTTON_SIZE, BUTTON_SIZE);
                else continue;

                InitializeLed(mapping[ledId], rectangle);
            }

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\Novation\Launchpads\{model.ToUpper()}.xml"), "Default", PathHelper.GetAbsolutePath(@"Images\Novation\Launchpads"));
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => LaunchpadIdMapping.DEFAULT[ledId];

        #endregion
    }
}