using System;

namespace RGB.NET.Core
{
    public partial class RGBSurface
    {
        #region EventHandler

        /// <summary>
        /// Represents the event-handler of the <see cref="Exception"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void ExceptionEventHandler(ExceptionEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="Updating"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void UpdatingEventHandler(UpdatingEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="Updated"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void UpdatedEventHandler(UpdatedEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="SurfaceLayoutChanged"/>-event.
        /// </summary>
        /// <param name="args"></param>
        public delegate void SurfaceLayoutChangedEventHandler(SurfaceLayoutChangedEventArgs args);

        #endregion

        #region Events

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <summary>
        /// Occurs when a catched exception is thrown inside the <see cref="RGBSurface"/>.
        /// </summary>
        public event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> starts updating.
        /// </summary>
        public event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> update is done.
        /// </summary>
        public event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the layout of this <see cref="RGBSurface"/> changed.
        /// </summary>
        public event SurfaceLayoutChangedEventHandler SurfaceLayoutChanged;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion

        #region Methods

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        private void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(new ExceptionEventArgs(ex));
            }
            catch { /* Well ... that's not my fault */ }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        private void OnUpdating(IUpdateTrigger trigger, CustomUpdateData customData)
        {
            try
            {
                double deltaTime = _deltaTimeCounter.Elapsed.TotalSeconds;
                _deltaTimeCounter.Restart();
                Updating?.Invoke(new UpdatingEventArgs(deltaTime, trigger, customData));
            }
            catch { /* Well ... that's not my fault */ }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        private void OnUpdated()
        {
            try
            {
                Updated?.Invoke(new UpdatedEventArgs());
            }
            catch { /* Well ... that's not my fault */ }
        }

        #endregion
    }
}
