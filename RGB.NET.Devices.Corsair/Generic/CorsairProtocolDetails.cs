// ReSharper disable MemberCanBePrivate.Global

using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Represents version information for the Corsair-SDK
/// </summary>
public sealed class CorsairSessionDetails
{
    #region Properties & Fields

    public string ClientVersion { get; }
    public string ServerVersion { get; }
    public string ServerHostVersion { get; }

    #endregion

    #region Constructors

    internal CorsairSessionDetails()
    {
        ClientVersion = string.Empty;
        ServerVersion = string.Empty;
        ServerHostVersion = string.Empty;
    }

    internal CorsairSessionDetails(_CorsairSessionDetails nativeDetails)
    {
        this.ClientVersion = nativeDetails.clientVersion.ToString();
        this.ServerVersion = nativeDetails.serverVersion.ToString();
        this.ServerHostVersion = nativeDetails.serverHostVersion.ToString();
    }

    #endregion
}