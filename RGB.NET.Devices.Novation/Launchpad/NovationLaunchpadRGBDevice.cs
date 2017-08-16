using System;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a Novation launchpad.
    /// </summary>
    public class NovationLaunchpadRGBDevice : NovationRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="NovationLaunchpadRGBDevice"/>.
        /// </summary>
        public NovationLaunchpadRGBDeviceInfo LaunchpadDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationLaunchpadRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by Novation for the launchpad</param>
        internal NovationLaunchpadRGBDevice(NovationLaunchpadRGBDeviceInfo info)
            : base(info)
        {
            this.LaunchpadDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //TODO DarthAffe 15.08.2017: Check if all launchpads are using the same basic layout
            //TODO DarthAffe 15.08.2017: Measure button size
            const int BUTTON_SIZE = 16;
            foreach (NovationLedIds ledId in Enum.GetValues(typeof(NovationLedIds)))
            {
                Rectangle rectangle;
                if (ledId.IsCustom())
                    rectangle = new Rectangle(BUTTON_SIZE * (ledId.GetId() - 0x68), 0, BUTTON_SIZE, BUTTON_SIZE);
                else if (ledId.IsScene())
                    rectangle = new Rectangle(8 * BUTTON_SIZE, BUTTON_SIZE * (((int)ledId.GetId() / 0x10) + 1), BUTTON_SIZE, BUTTON_SIZE);
                else if (ledId.IsGrid())
                    rectangle = new Rectangle(BUTTON_SIZE * ((int)ledId.GetId() % 0x10), BUTTON_SIZE * (((int)ledId.GetId() / 0x10) + 1), BUTTON_SIZE, BUTTON_SIZE);
                else continue;

                InitializeLed(new NovationLedId(this, ledId), rectangle);
            }

            string model = LaunchpadDeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\Novation\Launchpads\{model.ToUpper()}.xml"), "Default", PathHelper.GetAbsolutePath(@"Images\Novation\Launchpads"));
        }

        #endregion
    }
}