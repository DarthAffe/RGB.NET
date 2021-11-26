using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents an exception thrown by an <see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class RGBDeviceException : ApplicationException
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.RGBDeviceException" /> class.
    /// </summary>
    /// <param name="message">The message which describes the reason of throwing this exception.</param>
    /// <param name="innerException">Optional inner exception, which lead to this exception.</param>
    public RGBDeviceException(string message, Exception? innerException = null)
        : base(message, innerException)
    { }

    #endregion
}