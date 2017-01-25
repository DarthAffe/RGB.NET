// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents the information supplied with an <see cref="RGBSurface.Updating"/>-event.
    /// </summary>
    public class UpdatingEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the elapsed time (in seconds) since the last update.
        /// </summary>
        public double DeltaTime { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatingEventArgs"/> class.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public UpdatingEventArgs(double deltaTime)
        {
            this.DeltaTime = deltaTime;
        }

        #endregion
    }
}
