// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the information supplied with an <see cref="E:RGB.NET.Core.RGBSurface.Exception" />-event.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="System.Exception"/> which is responsible for the event-call.
        /// </summary>
        public Exception Exception { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.ExceptionEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The <see cref="T:System.Exception" /> which is responsible for the event-call.</param>
        public ExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        #endregion
    }
}
