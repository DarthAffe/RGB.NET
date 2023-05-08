using System;
using System.Collections.Generic;

namespace RGB.NET.Core;

public sealed class DevicesChangedEventArgs : EventArgs
{
    #region Properties & Fields

    public IList<IRGBDevice> Added { get; }
    public IList<IRGBDevice> Removed { get; }

    #endregion

    #region Constructors

    private DevicesChangedEventArgs(IList<IRGBDevice> added, IList<IRGBDevice> removed)
    {
        this.Added = added;
        this.Removed = removed;
    }

    #endregion

    #region Methods

    public static DevicesChangedEventArgs CreateDevicesAddedArgs(params IRGBDevice[] addedDevices) => CreateDevicesAddedArgs((IEnumerable<IRGBDevice>)addedDevices);
    public static DevicesChangedEventArgs CreateDevicesAddedArgs(IEnumerable<IRGBDevice> addedDevices) => new(new List<IRGBDevice>(addedDevices), new List<IRGBDevice>());

    public static DevicesChangedEventArgs CreateDevicesRemovedArgs(params IRGBDevice[] removedDevices) => CreateDevicesRemovedArgs((IEnumerable<IRGBDevice>)removedDevices);
    public static DevicesChangedEventArgs CreateDevicesRemovedArgs(IEnumerable<IRGBDevice> removedDevices) => new(new List<IRGBDevice>(), new List<IRGBDevice>(removedDevices));

    #endregion
}