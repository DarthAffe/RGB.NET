#pragma warning disable 1591
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using RGB.NET.Devices.Novation.Attributes;

namespace RGB.NET.Devices.Novation;

/// <summary>
/// Represents a specific novation device.
/// </summary>
public enum NovationDevices
{
    [DeviceId("Launchpad S")]
    [ColorCapability(NovationColorCapabilities.LimitedRG)]
    [LedIdMapping(LedIdMappings.Legacy)]
    LaunchpadS,

    [DeviceId("Launchpad Mini")]
    [ColorCapability(NovationColorCapabilities.LimitedRG)]
    [LedIdMapping(LedIdMappings.Legacy)]
    LaunchpadMini,

    [DeviceId("Launchpad MK2")]
    [ColorCapability(NovationColorCapabilities.RGB)]
    [LedIdMapping(LedIdMappings.Current)]
    LaunchpadMK2
}