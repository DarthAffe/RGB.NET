namespace RGB.NET.Core;

/// <summary>
/// Represents an update trigger used to trigger device-updates.
/// </summary>
public interface IDeviceUpdateTrigger : IUpdateTrigger
{
    /// <summary>
    /// Indicates that there's data available to process.
    /// </summary>
    void TriggerHasData();
}