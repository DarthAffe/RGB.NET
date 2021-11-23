using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.DMX.E131;

/// <summary>
/// Represents a E1.31-DXM-device.
/// </summary>
public class E131Device : AbstractRGBDevice<E131DeviceInfo>, IUnknownDevice
{
    #region Properties & Fields

    private readonly Dictionary<LedId, List<(int channel, Func<Color, byte> getValueFunc)>> _ledMappings;

    #endregion

    #region Constructors

    /// <inheritdoc />
    internal E131Device(E131DeviceInfo deviceInfo, Dictionary<LedId, List<(int channel, Func<Color, byte> getValueFunc)>> ledMappings, IDeviceUpdateTrigger updateTrigger)
        : base(deviceInfo, new E131UpdateQueue(updateTrigger, deviceInfo.Hostname, deviceInfo.Port))
    {
        this._ledMappings = ledMappings;

        InitializeLayout();

        E131UpdateQueue updateQueue = (E131UpdateQueue)UpdateQueue;
        updateQueue.DataPacket.SetCID(DeviceInfo.CID);
        updateQueue.DataPacket.SetUniverse(DeviceInfo.Universe);
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        int count = 0;
        foreach (LedId id in _ledMappings.Keys)
            AddLed(id, new Point((count++) * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => new LedChannelMapping(_ledMappings[ledId]);

    #endregion
}