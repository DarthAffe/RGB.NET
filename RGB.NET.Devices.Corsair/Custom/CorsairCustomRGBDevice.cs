// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a corsair custom.
    /// </summary>
    public class CorsairCustomRGBDevice : CorsairRGBDevice<CorsairCustomRGBDeviceInfo>, IUnknownDevice
    {
        #region Properties & Fields

        private readonly Dictionary<LedId, CorsairLedId> _idMapping = new();

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairCustomRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the custom-device.</param>
        internal CorsairCustomRGBDevice(CorsairCustomRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            LedId referenceId = GetReferenceLed(DeviceInfo.DeviceType);

            for (int i = 0; i < DeviceInfo.LedCount; i++)
            {
                LedId ledId = referenceId + i;
                _idMapping.Add(ledId, DeviceInfo.ReferenceCorsairLed + i);
                AddLed(ledId, new Point(i * 10, 0), new Size(10, 10));
            }
        }

        protected override object GetLedCustomData(LedId ledId) => _idMapping.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;

        protected virtual LedId GetReferenceLed(RGBDeviceType deviceType)
        {
            switch (deviceType)
            {
                case RGBDeviceType.LedStripe:
                    return LedId.LedStripe1;

                case RGBDeviceType.Fan:
                    return LedId.Fan1;

                case RGBDeviceType.Cooler:
                    return LedId.Cooler1;

                default:
                    return LedId.Custom1;
            }
        }

        #endregion
    }
}
