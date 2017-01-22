using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic RGB-surface.
    /// </summary>
    public class RGBSurface
    {
        #region Properties & Fields

        private DateTime _lastUpdate;

        #endregion

        #region Events

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <summary>
        /// Occurs when a catched exception is thrown inside the <see cref="IRGBSurface"/>.
        /// </summary>
        public event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> starts updating.
        /// </summary>
        public event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> update is done.
        /// </summary>
        public event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> starts to update the <see cref="Led"/>.
        /// </summary>
        public event LedsUpdatingEventHandler LedsUpdating;

        /// <summary>
        /// Occurs when the <see cref="IRGBSurface"/> updated the <see cref="Led"/>.
        /// </summary>
        public event LedsUpdatedEventHandler LedsUpdated;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBSurface"/> class.
        /// </summary>
        public RGBSurface()
        {
            _lastUpdate = DateTime.Now;
        }

        #endregion

        #region EventCaller

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        protected virtual void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(this, new ExceptionEventArgs(ex));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        protected virtual void OnUpdating()
        {
            try
            {
                long lastUpdateTicks = _lastUpdate.Ticks;
                _lastUpdate = DateTime.Now;
                Updating?.Invoke(this, new UpdatingEventArgs((DateTime.Now.Ticks - lastUpdateTicks) / 10000000.0));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        protected virtual void OnUpdated()
        {
            try
            {
                Updated?.Invoke(this, new UpdatedEventArgs());
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before the <see cref="Led"/> are updated.
        /// </summary>
        protected virtual void OnLedsUpdating(ICollection<Led> updatingLeds)
        {
            try
            {
                LedsUpdating?.Invoke(this, new LedsUpdatingEventArgs(updatingLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after the <see cref="Led"/> are updated.
        /// </summary>
        protected virtual void OnLedsUpdated(IEnumerable<Led> updatedLeds)
        {
            try
            {
                LedsUpdated?.Invoke(this, new LedsUpdatedEventArgs(updatedLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        #endregion
    }
}
