using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech;

/// <inheritdoc cref="LogitechRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a logitech zone-lightable device.
/// </summary>
public class LogitechZoneRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>, IUnknownDevice //TODO DarthAffe 18.04.2020: It's know which kind of device this is, but they would need to be separated
{
    #region Properties & Fields

    private readonly LedMapping<int> _ledMapping;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechZoneRGBDevice" /> class.
    /// </summary>
    internal LogitechZoneRGBDevice(LogitechRGBDeviceInfo info, IUpdateQueue updateQueue, LedMapping<int> ledMapping)
        : base(info, updateQueue)
    {
        this._ledMapping = ledMapping;

        InitializeLayout();
    }

    #endregion

    #region Methods
    private void InitializeLayout()
    {
        for (int i = DeviceInfo.ZoneOffset; i < (DeviceInfo.ZoneOffset + DeviceInfo.Zones); i++)
            AddLed(_ledMapping[i], new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledMapping[ledId];

    /// <inheritdoc />
    protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

    #endregion
}