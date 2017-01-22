namespace RGB.NET.Core
{

    #region EventHandler

    /// <summary>
    /// Represents the event-handler of the <see cref="IRGBSurface.Exception"/>-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs args);

    /// <summary>
    /// Represents the event-handler of the <see cref="IRGBSurface.Updating"/>-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void UpdatingEventHandler(object sender, UpdatingEventArgs args);

    /// <summary>
    /// Represents the event-handler of the <see cref="IRGBSurface.Updated"/>-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void UpdatedEventHandler(object sender, UpdatedEventArgs args);

    /// <summary>
    /// Represents the event-handler of the <see cref="IRGBSurface.LedsUpdating"/>-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void LedsUpdatingEventHandler(object sender, LedsUpdatingEventArgs args);

    /// <summary>
    /// Represents the event-handler of the <see cref="IRGBSurface.LedsUpdated"/>-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void LedsUpdatedEventHandler(object sender, LedsUpdatedEventArgs args);

    #endregion

    /// <summary>
    /// Represents a generic RGB-surface.
    /// </summary>
    public interface IRGBSurface
    {
        #region Events

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <summary>
        /// Occurs when a catched exception is thrown inside the <see cref="IRGBSurface"/>.
        /// </summary>
        event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> starts updating.
        /// </summary>
        event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> update is done.
        /// </summary>
        event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> starts to update the leds.
        /// </summary>
        event LedsUpdatingEventHandler LedsUpdating;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> updated the leds.
        /// </summary>
        event LedsUpdatedEventHandler LedsUpdated;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion
    }
}
