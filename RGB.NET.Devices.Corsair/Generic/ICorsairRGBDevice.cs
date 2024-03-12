﻿using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Represents a corsair RGB-device.
/// </summary>
public interface ICorsairRGBDevice : IRGBDevice
{
    internal string DeviceId { get; }

    internal void Initialize();
}