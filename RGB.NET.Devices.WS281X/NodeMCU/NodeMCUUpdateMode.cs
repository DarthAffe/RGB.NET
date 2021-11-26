namespace RGB.NET.Devices.WS281X.NodeMCU;

/// <summary>
/// Contaisn a list of possible update-modes for NodeMCU-devices.
/// </summary>
// ReSharper disable once InconsistentNaming
public enum NodeMCUUpdateMode
{
    /// <summary>
    /// Updates through the HTTP-REST-API.
    /// Slow, but reliable.
    /// </summary>
    Http, 

    /// <summary>
    /// Updates through a UDP-connection.
    /// Fast, but might skip updates if the network connection is bad.
    /// </summary>
    Udp
}