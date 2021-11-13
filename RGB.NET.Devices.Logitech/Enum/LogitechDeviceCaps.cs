// ReSharper disable InconsistentNaming

using System;

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.Logitech;

[Flags]
public enum LogitechDeviceCaps
{
    None = 0,
    Monochrome = 1 << 0,
    DeviceRGB = 1 << 1,
    PerKeyRGB = 1 << 2,
    All = Monochrome | DeviceRGB | PerKeyRGB
}