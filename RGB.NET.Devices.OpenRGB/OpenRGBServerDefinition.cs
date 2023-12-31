namespace RGB.NET.Devices.OpenRGB;

/// <summary>
/// Represents a definition of an OpenRGB server.
/// </summary>
public sealed class OpenRGBServerDefinition
{
    /// <summary>
    /// The name of the client that will appear in the OpenRGB interface.
    /// </summary>
    public string ClientName { get; set; } = "RGB.NET";

    /// <summary>
    /// The ip address of the server.
    /// </summary>
    public string Ip { get; set; } = "127.0.0.1";

    /// <summary>
    /// The port of the server.
    /// </summary>
    public int Port { get; set; } = 6742;

    /// <summary>
    /// Whether the provider is connected to this server definition or not.
    /// </summary>
    public bool Connected { get; set; }

    /// <summary>
    /// The error that occurred when connecting, if this failed.
    /// </summary>
    public string? LastError { get; set; }
}