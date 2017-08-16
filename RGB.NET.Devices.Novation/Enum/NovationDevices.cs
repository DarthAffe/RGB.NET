#pragma warning disable 1591
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using RGB.NET.Devices.Novation.Attributes;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a specific novation device.
    /// </summary>
    public enum NovationDevices
    {
        [DeviceId("Launchpad S")]
        [ColorCapability(NovationColorCapabilities.LimitedRG)]
        LaunchpadS
    }
}
