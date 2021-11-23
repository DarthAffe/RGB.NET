using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a trigger causing an update.
/// </summary>
public interface IUpdateTrigger : IDisposable
{
    /// <summary>
    /// Gets the time spent for the last update.
    /// </summary>
    double LastUpdateTime { get; }

    /// <summary>
    /// Occurs when the trigger is starting up.
    /// </summary>
    event EventHandler<CustomUpdateData>? Starting;

    /// <summary>
    /// Occurs when the trigger wants to cause an update.
    /// </summary>
    event EventHandler<CustomUpdateData>? Update;

    /// <summary>
    /// Starts the update trigger.
    /// </summary>
    void Start();
}