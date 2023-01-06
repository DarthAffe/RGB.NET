// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair custom.
/// </summary>
public class CorsairCustomRGBDevice : CorsairRGBDevice<CorsairCustomRGBDeviceInfo>, IUnknownDevice
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairCustomRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the custom-device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairCustomRGBDevice(CorsairCustomRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, new LedMapping<CorsairLedId>(), updateQueue)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeLayout()
    {
        Mapping.Clear();

        _CorsairLedPositions? nativeLedPositions = (_CorsairLedPositions?)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(DeviceInfo.CorsairDeviceIndex), typeof(_CorsairLedPositions));
        if (nativeLedPositions == null) return;

        int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
        IntPtr ptr = new(nativeLedPositions.pLedPosition.ToInt64() + (structSize * DeviceInfo.LedOffset));

        LedId referenceLedId = GetReferenceLed(DeviceInfo.DeviceType);
        for (int i = 0; i < DeviceInfo.LedCount; i++)
        {
            LedId ledId = referenceLedId + i;
            _CorsairLedPosition? ledPosition = (_CorsairLedPosition?)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
            if (ledPosition == null)
            {
                ptr = new IntPtr(ptr.ToInt64() + structSize);
                continue;
            }

            Mapping.Add(ledId, ledPosition.LedId);

            Rectangle rectangle = ledPosition.ToRectangle();
            AddLed(ledId, rectangle.Location, rectangle.Size);

            ptr = new IntPtr(ptr.ToInt64() + structSize);
        }
    }

    private static LedId GetReferenceLed(RGBDeviceType deviceType)
        => deviceType switch
        {
            RGBDeviceType.LedStripe => LedId.LedStripe1,
            RGBDeviceType.Fan => LedId.Fan1,
            RGBDeviceType.Cooler => LedId.Cooler1,
            _ => LedId.Custom1
        };

    #endregion
}