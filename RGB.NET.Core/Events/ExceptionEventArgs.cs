// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core;

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

    /// <summary>
    /// Gets a bool indicating if the exception is critical for the thrower.
    /// </summary>
    public bool IsCritical { get; }

    /// <summary>
    /// Gets or sets if the exception should be thrown after the event is handled.
    /// </summary>
    public bool Throw { get; set; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.ExceptionEventArgs" /> class.
    /// </summary>
    /// <param name="exception">The <see cref="T:System.Exception" /> which is responsible for the event-call.</param>
    /// <param name="isCritical">Indicates if the exception is critical for the thrower.</param>
    /// <param name="throw">Indicates if the exception should be thrown after the event is handled.</param>
    public ExceptionEventArgs(Exception exception, bool isCritical = false, bool @throw = false)
    {
        this.Exception = exception;
        this.IsCritical = isCritical;
        this.Throw = @throw;
    }

    #endregion
}