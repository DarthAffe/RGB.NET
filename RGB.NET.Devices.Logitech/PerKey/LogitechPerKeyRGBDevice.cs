using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech;

/// <inheritdoc cref="LogitechRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a logitech per-key-lightable device.
/// </summary>
public sealed class LogitechPerKeyRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>, IUnknownDevice //TODO DarthAffe 18.04.2020: It's know which kind of device this is, but they would need to be separated
{
    #region Properties & Fields

    private readonly LedMapping<LogitechLedId> _ledMapping;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechPerKeyRGBDevice" /> class.
    /// </summary>
    internal LogitechPerKeyRGBDevice(LogitechRGBDeviceInfo info, IUpdateQueue updateQueue, LedMapping<LogitechLedId> ledMapping)
        : base(info, updateQueue)
    {
        this._ledMapping = ledMapping;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        int pos = 0;
            
        foreach ((LedId ledId, LogitechLedId mapping) in _ledMapping)
            AddLed(ledId, new Point(pos++ * 19, 0), new Size(19, 19));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledMapping.TryGetValue(ledId, out LogitechLedId logitechLedId) ? logitechLedId : -1;

    #endregion
}