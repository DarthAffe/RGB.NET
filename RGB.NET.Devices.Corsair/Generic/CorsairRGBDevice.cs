using System.Collections.Generic;
using System.Linq;
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
    protected LedMapping<CorsairLedId> Mapping { get; private set; } = LedMapping<CorsairLedId>.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsairRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by CUE for the device.</param>
    /// <param name="mapping">The mapping <see cref="LedId"/> to <see cref="CorsairLedId"/> used to update the LEDs of this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected CorsairRGBDevice(TDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    void ICorsairRGBDevice.Initialize() => InitializeLayout();

    /// <summary>
    /// Initializes the LEDs of the device based on the data provided by the SDK.
    /// </summary>
    protected virtual void InitializeLayout()
    {
        CorsairError error = _CUESDK.CorsairGetLedPositions(DeviceInfo.DeviceId, out _CorsairLedPosition[] ledPositions);
        if (error != CorsairError.Success)
            throw new RGBDeviceException($"Failed to load device '{DeviceInfo.DeviceId}'. (ErrorCode: {error})");

        List<_CorsairLedPosition> deviceLeds = ledPositions.Skip(DeviceInfo.LedOffset).Take(DeviceInfo.LedCount).ToList();

        Mapping = CreateMapping(deviceLeds.Select(x => new CorsairLedId(x.id)));

        foreach (_CorsairLedPosition ledPosition in deviceLeds)
        {
            LedId ledId = Mapping.TryGetValue(new CorsairLedId(ledPosition.id), out LedId id) ? id : LedId.Invalid;
            Rectangle rectangle = ledPosition.ToRectangle();
            AddLed(ledId, rectangle.Location, rectangle.Size);
        }

        if (DeviceInfo.LedOffset > 0)
            FixOffsetDeviceLayout();
    }

    /// <summary>
    /// Fixes the locations for devices split by offset by aligning them to the top left.
    /// </summary>
    protected virtual void FixOffsetDeviceLayout()
    {
        float minX = this.Min(x => x.Location.X);
        float minY = this.Min(x => x.Location.Y);

        foreach (Led led in this)
            led.Location = led.Location.Translate(-minX, -minY);
    }

    protected abstract LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids);

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => Mapping.TryGetValue(ledId, out CorsairLedId corsairLedId) ? corsairLedId : new CorsairLedId(0);

    #endregion
}