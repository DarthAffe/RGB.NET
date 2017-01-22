using System.Collections.Generic;

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

    #endregion

    /// <summary>
    /// Represents a generic RGB-surface.
    /// </summary>
    public interface IRGBSurface : ILedGroup
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a dictionary containing the locations of all <see cref="IRGBDevice"/> positioned on this <see cref="IRGBSurface"/>.
        /// </summary>
        Dictionary<IRGBDevice, Point> Devices { get; }

        /// <summary>
        /// Gets a copy of the <see cref="Rectangle"/> representing this <see cref="IRGBSurface"/>.
        /// </summary>
        Rectangle SurfaceRectangle { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Perform an update for all dirty <see cref="Led"/>, or all <see cref="Led"/>, if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all <see cref="Led"/>, (including clean ones) should be updated.</param>
        void Update(bool flushLeds = false);

        /// <summary>
        /// Sets the location of the given <see cref="IRGBDevice"/> to the given <see cref="Point"/>.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> to move.</param>
        /// <param name="location">The target <see cref="PositionDevice"/>.</param>
        void PositionDevice(IRGBDevice device, Point location);

        /// <summary>
        /// Attaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        bool AttachLedGroup(ILedGroup ledGroup);

        /// <summary>
        /// Detaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to detached.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be detached; otherwise, <c>false</c>.</returns>
        bool DetachLedGroup(ILedGroup ledGroup);

        #endregion

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

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion
    }
}
