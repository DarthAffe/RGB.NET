// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc />
/// <summary>
/// Represents an exception thrown by the Razer-SDK.
/// </summary>
public class RazerException : ApplicationException
{
    #region Properties & Fields

    /// <summary>
    /// Gets the error code provided by the SDK.
    /// </summary>
    public RazerError ErrorCode { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerException" /> class.
    /// </summary>
    /// <param name="errorCode">The error code provided by the SDK.</param>
    public RazerException(RazerError errorCode)
    {
        this.ErrorCode = errorCode;
    }

    #endregion
}