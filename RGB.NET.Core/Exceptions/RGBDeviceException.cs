using System;

namespace RGB.NET.Core.Exceptions
{
    /// <summary>
    /// Represents an exception thrown by an <see cref="IRGBDevice"/>.
    /// </summary>
    public class RGBDeviceException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBDeviceException"/> class.
        /// </summary>
        /// <param name="message">The message which describes the reason of throwing this exception.</param>
        /// <param name="innerException">Optional inner exception, which lead to this exception.</param>
        public RGBDeviceException(string message, Exception innerException = null)
            : base(message, innerException)
        { }

        #endregion
    }
}
