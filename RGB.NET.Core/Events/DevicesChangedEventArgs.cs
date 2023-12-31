using System;

namespace RGB.NET.Core;

public sealed class DevicesChangedEventArgs(IRGBDevice device, DevicesChangedEventArgs.DevicesChangedAction action)
    : EventArgs
{
    #region Properties & Fields

    public IRGBDevice Device { get; } = device;
    public DevicesChangedAction Action { get; } = action;

    #endregion

    #region Methods

    public static DevicesChangedEventArgs CreateDevicesAddedArgs(IRGBDevice addedDevice) => new(addedDevice, DevicesChangedAction.Added);
    public static DevicesChangedEventArgs CreateDevicesRemovedArgs(IRGBDevice removedDevice) => new(removedDevice, DevicesChangedAction.Removed);

    #endregion

    public enum DevicesChangedAction
    {
        Added,
        Removed
    }
}