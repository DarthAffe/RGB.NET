using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <summary>
/// Represents a DynamicLighting RGB-device.
/// </summary>
public interface IDynamicLightingRGBDevice : IRGBDevice
{
    internal void Initialize();
}