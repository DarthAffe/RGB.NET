using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;

namespace RGB.NET.Devices.Wooting.Generic;

public abstract class WootingRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IWootingRGBDevice
    where TDeviceInfo : WootingRGBDeviceInfo
{
    #region Properties & Fields

    private readonly Dictionary<LedId, (int row, int column)> _mapping;

    #endregion

    #region Constructors

    internal WootingRGBDevice(WootingDeviceType deviceType, TDeviceInfo info, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    {
        _mapping = WootingLedMappings.Mapping[deviceType];
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        foreach (KeyValuePair<LedId, (int row, int column)> led in _mapping)
            AddLed(led.Key, new Point(led.Value.column * 19, led.Value.row * 19), new Size(19, 19));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _mapping[ledId];
    
    /// <inheritdoc />
    public override void Dispose()
    {
        UpdateQueue.Dispose();

        base.Dispose();
    }

    #endregion
}
