using System;

namespace RGB.NET.Core;

public sealed class DevicesChangedEventArgs : EventArgs
{
    #region Properties & Fields

    public IRGBDevice Device { get; }
    public DevicesChangedAction Action { get; }

    #endregion

    #region Constructors

    public DevicesChangedEventArgs(IRGBDevice device, DevicesChangedAction action)
    {
        this.Device = device;
        this.Action = action;
    }

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