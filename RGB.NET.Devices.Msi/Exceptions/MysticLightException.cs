// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System;

namespace RGB.NET.Devices.Msi.Exceptions;

/// <inheritdoc />
/// <summary>
/// Represents an exception thrown by the MysticLight-SDK.
/// </summary>
public class MysticLightException : ApplicationException
{
    #region Properties & Fields

    /// <summary>
    /// Gets the raw error code provided by the SDK.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Gets the text-description the <see cref="ErrorCode"/> resolves too.
    /// </summary>
    public string Description { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Msi.MysticLightException" /> class.
    /// </summary>
    /// <param name="errorCode">The raw error code provided by the SDK.</param>
    /// <param name="description">The text-description of the error.</param>
    public MysticLightException(int errorCode, string description)
        : base($"MSI error code {errorCode} ({description})")
    {
        this.ErrorCode = errorCode;
        this.Description = description;
    }

    #endregion
}