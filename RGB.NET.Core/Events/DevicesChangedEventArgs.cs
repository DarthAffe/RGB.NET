using System;

namespace RGB.NET.Core;

public sealed class DevicesChangedEventArgs : EventArgs
{
    #region Properties & Fields

    public IRGBDevice? Added { get; }
    public IRGBDevice? Removed { get; }

    #endregion

    #region Constructors

    private DevicesChangedEventArgs(IRGBDevice? added, IRGBDevice? removed)
    {
        this.Added = added;
        this.Removed = removed;
    }

    #endregion

    #region Methods

    public static DevicesChangedEventArgs CreateDevicesAddedArgs(IRGBDevice addedDevice) => new(addedDevice, null);
    public static DevicesChangedEventArgs CreateDevicesRemovedArgs(IRGBDevice removedDevice) => new(null, removedDevice);

    #endregion
}