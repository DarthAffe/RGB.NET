using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc cref="NovationRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a Novation launchpad.
    /// </summary>
    public class NovationLaunchpadRGBDevice : NovationRGBDevice<NovationLaunchpadRGBDeviceInfo>, ILedMatrix
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
            Dictionary<LedId, (byte mode, byte id, int x, int y)> mapping = GetDeviceMapping();

            const int BUTTON_SIZE = 20;
            foreach (LedId ledId in mapping.Keys)
            {
                (_, _, int x, int y) = mapping[ledId];
                InitializeLed(ledId, new Point(BUTTON_SIZE * x, BUTTON_SIZE * y), new Size(BUTTON_SIZE));
            }

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, @"Layouts\Novation\Launchpads", $"{model.ToUpper()}.xml"), "Default");
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => GetDeviceMapping().TryGetValue(ledId, out (byte mode, byte id, int _, int __) data) ? (data.mode, data.id) : ((byte)0x00, (byte)0x00);

        protected virtual Dictionary<LedId, (byte mode, byte id, int x, int y)> GetDeviceMapping()
            => DeviceInfo.LedIdMapping switch
            {
                LedIdMappings.Current => LaunchpadIdMapping.CURRENT,
                LedIdMappings.Legacy => LaunchpadIdMapping.LEGACY,
                _ => throw new ArgumentOutOfRangeException()
            };

        #endregion
    }
}