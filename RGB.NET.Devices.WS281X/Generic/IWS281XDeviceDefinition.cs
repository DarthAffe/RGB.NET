using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X;

/// <summary>
/// Marker interface for WS281X device definitions.
/// </summary>
// ReSharper disable once InconsistentNaming
public interface IWS281XDeviceDefinition
{
    /// <summary>
    /// Gets the devices defined by this definition.
    /// </summary>
    /// <returns>The initialized devices defined by this definition.</returns>
    IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger);
}