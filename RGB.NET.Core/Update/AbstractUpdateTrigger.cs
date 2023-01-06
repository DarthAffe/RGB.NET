using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic update trigger. 
/// </summary>
public abstract class AbstractUpdateTrigger : AbstractBindable, IUpdateTrigger
{
    #region Properties & Fields

    /// <inheritdoc />
    public abstract double LastUpdateTime { get; protected set; }

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler<CustomUpdateData>? Starting;
    /// <inheritdoc />
    public event EventHandler<CustomUpdateData>? Update;

    #endregion

    #region Methods

    /// <summary>
    /// Invokes the <see cref="Starting"/>-event.
    /// </summary>
    /// <param name="updateData">Optional custom-data passed to the subscribers of the <see cref="Starting"/>.event.</param>
    protected virtual void OnStartup(CustomUpdateData? updateData = null) => Starting?.Invoke(this, updateData ?? new CustomUpdateData());

    /// <summary>
    /// Invokes the <see cref="Update"/>-event.
    /// </summary>
    /// <param name="updateData">Optional custom-data passed to the subscribers of the <see cref="Update"/>.event.</param>
    protected virtual void OnUpdate(CustomUpdateData? updateData = null) => Update?.Invoke(this, updateData ?? new CustomUpdateData());

    /// <inheritdoc />
    public abstract void Start();

    /// <inheritdoc />
    public abstract void Dispose();

    #endregion
}