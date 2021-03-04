// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a corsair headset stand.
    /// </summary>
    public class CorsairHeadsetStandRGBDevice : CorsairRGBDevice<CorsairHeadsetStandRGBDeviceInfo>, IHeadsetStand
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetStandRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset stand</param>
        internal CorsairHeadsetStandRGBDevice(CorsairHeadsetStandRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            _CorsairLedPositions? nativeLedPositions = (_CorsairLedPositions?)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(DeviceInfo.CorsairDeviceIndex), typeof(_CorsairLedPositions));
            if (nativeLedPositions == null) return;

            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;

            List<_CorsairLedPosition> positions = new();
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition? ledPosition = (_CorsairLedPosition?)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                if (ledPosition == null) continue;

                ptr = new IntPtr(ptr.ToInt64() + structSize);
                positions.Add(ledPosition);
            }

            Dictionary<CorsairLedId, LedId> mapping = HeadsetStandIdMapping.DEFAULT.SwapKeyValue();
            foreach (_CorsairLedPosition ledPosition in positions.OrderBy(p => p.LedId))
            {
                LedId ledId = mapping.TryGetValue(ledPosition.LedId, out LedId id) ? id : LedId.Invalid;
                Rectangle rectangle = ledPosition.ToRectangle();
                AddLed(ledId, rectangle.Location, rectangle.Size);
            }
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => HeadsetStandIdMapping.DEFAULT.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;

        #endregion
    }
}
