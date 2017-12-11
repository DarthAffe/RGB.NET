// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Razer.RazerMousepadRGBDevice" />.
    /// </summary>
    public class RazerMousepadRGBDeviceInfo : RazerRGBDeviceInfo
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Razer.RazerMousepadRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceId">The Id of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
        internal RazerMousepadRGBDeviceInfo(Guid deviceId, string model)
            : base(deviceId, RGBDeviceType.Mousepad, model)
        {
            string modelName = Model.Replace(" ", string.Empty).ToUpper();
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Razer\Mousepads\{modelName}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
