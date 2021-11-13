using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic CUE-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class CorsairRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICorsairRGBDevice
    where TDeviceInfo : CorsairRGBDeviceInfo
{
    #region Properties & Fields

    /// <summary>
    /// Gets the mapping of <see cref="LedId"/> to <see cref="CorsairLedId"/> used to update the LEDs of this device.
    /// </summary>
    protected LedMapping<CorsairLedId> Mapping { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsairRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by CUE for the device.</param>
    /// <param name="mapping">The mapping <see cref="LedId"/> to <see cref="CorsairLedId"/> used to update the LEDs of this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected CorsairRGBDevice(TDeviceInfo info, LedMapping<CorsairLedId> mapping, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    {
        this.Mapping = mapping;
    }

    #endregion

    #region Methods

    void ICorsairRGBDevice.Initialize() => InitializeLayout();
        
    /// <summary>
    /// Initializes the LEDs of the device based on the data provided by the SDK.
    /// </summary>
    protected virtual void InitializeLayout()
    {
        _CorsairLedPositions? nativeLedPositions = (_CorsairLedPositions?)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(DeviceInfo.CorsairDeviceIndex), typeof(_CorsairLedPositions));
        if (nativeLedPositions == null) return;

        int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
        IntPtr ptr = nativeLedPositions.pLedPosition;

        for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
        {
            _CorsairLedPosition? ledPosition = (_CorsairLedPosition?)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
            if (ledPosition == null)
            {
                ptr = new IntPtr(ptr.ToInt64() + structSize);
                continue;
            }

            LedId ledId = Mapping.TryGetValue(ledPosition.LedId, out LedId id) ? id : LedId.Invalid;
            Rectangle rectangle = ledPosition.ToRectangle();
            AddLed(ledId, rectangle.Location, rectangle.Size);

            ptr = new IntPtr(ptr.ToInt64() + structSize);
        }
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => Mapping.TryGetValue(ledId, out CorsairLedId corsairLedId) ? corsairLedId : CorsairLedId.Invalid;

    #endregion
}