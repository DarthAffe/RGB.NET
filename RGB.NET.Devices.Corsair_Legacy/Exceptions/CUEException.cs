// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System;

namespace RGB.NET.Devices.CorsairLegacy;

/// <inheritdoc />
/// <summary>
/// Represents an exception thrown by the CUE.
/// </summary>
public class CUEException : ApplicationException
{
    #region Properties & Fields

    /// <summary>
    /// Gets the <see cref="CorsairError" /> provided by CUE.
    /// </summary>
    public CorsairError Error { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.CorsairLegacy.CUEException" /> class.
    /// </summary>
    /// <param name="error">The <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairError" /> provided by CUE, which leads to this exception.</param>
    public CUEException(CorsairError error)
    {
        this.Error = error;
    }

    #endregion
}