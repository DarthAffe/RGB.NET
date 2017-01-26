using System;

namespace RGB.NET.Core
{
    public static partial class RGBSurface
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
        public static event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> starts updating.
        /// </summary>
        public static event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> update is done.
        /// </summary>
        public static event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the layout of this <see cref="RGBSurface"/> changed.
        /// </summary>
        public static event SurfaceLayoutChangedEventHandler SurfaceLayoutChanged;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion

        #region Methods

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        private static void OnException(Exception ex)
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
        private static void OnUpdating()
        {
            try
            {
                long lastUpdateTicks = _lastUpdate.Ticks;
                _lastUpdate = DateTime.Now;
                Updating?.Invoke(new UpdatingEventArgs((DateTime.Now.Ticks - lastUpdateTicks) / 10000000.0));
            }
            catch { /* Well ... that's not my fault */ }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        private static void OnUpdated()
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
