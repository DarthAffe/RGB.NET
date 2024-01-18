using System.Net.Http;
using System.Net.Http.Json;

namespace RGB.NET.Devices.WLED;

/// <summary>
/// Partial implementation of the WLED-JSON-API
/// </summary>
public static class WledAPI
{
    /// <summary>
    /// Gets the data returned by the 'info' endpoint of the WLED-device.
    /// </summary>
    /// <param name="address">The address of the device to request data from.</param>
    /// <returns>The data returned by the WLED-device.</returns>
    public static WledInfo? Info(string address)
    {
        if (string.IsNullOrEmpty(address)) return null;

        using HttpClient client = new();
        try
        {
            return client.Send(new HttpRequestMessage(HttpMethod.Get, $"http://{address}/json/info"))
                         .Content
                         .ReadFromJsonAsync<WledInfo>()
                         .Result;
        }
        catch
        {
            return null;
        }
    }
}