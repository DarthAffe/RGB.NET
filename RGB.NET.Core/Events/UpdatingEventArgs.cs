// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core
{
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

        public IUpdateTrigger Trigger { get; }

        public CustomUpdateData CustomData { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.UpdatingEventArgs" /> class.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public UpdatingEventArgs(double deltaTime, IUpdateTrigger trigger, CustomUpdateData customData)
        {
            this.DeltaTime = deltaTime;
            this.Trigger = trigger;
            this.CustomData = customData;
        }

        #endregion
    }
}
