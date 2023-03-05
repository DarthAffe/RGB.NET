using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Debug;

internal class DebugDeviceUpdateQueue : UpdateQueue
{
    #region Constructors

    public DebugDeviceUpdateQueue()
        : base(new DeviceUpdateTrigger())
    { }

    #endregion

    #region Methods

    protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet) => true;

    #endregion
}