using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents an exception thrown by a <see cref="IRGBDeviceProvider" />.
/// </summary>
public class DeviceProviderException : ApplicationException
{
    #region Properties & Fields

    /// <summary>
    /// Gets a bool indicating if the exception is critical and shouldn't be ingored.
    /// </summary>
    public bool IsCritical { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceProviderException" /> class.
    /// </summary>
    /// <param name="innerException">The exception that is the casue of the current exception or null if this exception was thrown on purpose.</param>
    /// <param name="isCritical">A value indicating if the exception is critical and shouldn't be ignored.</param>
    public DeviceProviderException(Exception? innerException, bool isCritical)
        : base(innerException?.Message, innerException)
    {
        this.IsCritical = isCritical;
    }

    #endregion
}