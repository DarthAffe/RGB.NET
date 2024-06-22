using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Debug;

internal sealed class DebugDeviceUpdateQueue() : UpdateQueue(new DeviceUpdateTrigger())
{
    #region Methods

    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet) => true;

    #endregion
}