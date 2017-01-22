using System;

namespace RGB.NET.Core.Exceptions
{
    /// <summary>
    /// Represents an exception thrown by an <see cref="IEffect"/>.
    /// </summary>
    public class EffectException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectException"/> class.
        /// </summary>
        /// <param name="message">The message which describes the reason of throwing this exception.</param>
        /// <param name="innerException">Optional inner exception, which lead to this exception.</param>
        public EffectException(string message, Exception innerException = null)
            : base(message, innerException)
        { }

        #endregion
    }
}
