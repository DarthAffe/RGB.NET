// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents the information supplied with an <see cref="E:RGB.NET.Core.RGBSurface.Updating" />-event.
/// </summary>
public class UpdatingEventArgs : EventArgs
{
    #region Properties & Fields

    /// <summary>
    /// Gets the elapsed time (in seconds) since the last update.
    /// </summary>
    public double DeltaTime { get; }

    /// <summary>
    /// Gets the trigger causing this update.
    /// </summary>
    public IUpdateTrigger? Trigger { get; }

    /// <summary>
    /// Gets the custom-data provided by the trigger for this update.
    /// </summary>
    public CustomUpdateData CustomData { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.UpdatingEventArgs" /> class.
    /// </summary>
    /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
    /// <param name="trigger">The trigger causing this update.</param>
    /// <param name="customData">The custom-data provided by the trigger for this update.</param>
    public UpdatingEventArgs(double deltaTime, IUpdateTrigger? trigger, CustomUpdateData customData)
    {
        this.DeltaTime = deltaTime;
        this.Trigger = trigger;
        this.CustomData = customData;
    }

    #endregion
}