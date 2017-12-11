// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Globalization;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDevice" />.
    /// </summary>
    public class RazerKeyboardRGBDeviceInfo : RazerRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public RazerPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the logical layout of the keyboard as set in CUE settings.
        /// </summary>
        public RazerLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceId">The Id of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the layout this keyboard is using.</param>
        internal RazerKeyboardRGBDeviceInfo(Guid deviceId, string model, CultureInfo culture)
            : base(deviceId, RGBDeviceType.Keyboard, model)
        {
            SetLayouts(culture.KeyboardLayoutId);

            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Razer\Keyboards\{Model.Replace(" ", string.Empty).ToUpper()}.png"),
                            UriKind.Absolute);
        }

        #endregion

        #region Methods

        private void SetLayouts(int keyboardLayoutId)
        {
            switch (keyboardLayoutId)
            {
                //TODO DarthAffe 07.10.2017: Implement
                default:
                    PhysicalLayout = RazerPhysicalKeyboardLayout.TODO;
                    LogicalLayout = RazerLogicalKeyboardLayout.TODO;
                    break;
            }
        }

        #endregion
    }
}
