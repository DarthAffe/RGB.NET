using System.Collections.Generic;
using OpenRGB.NET;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB;

public class OpenRGBClientWrapper
{
    public OpenRgbClient OpenRgbClient { get; }

    public List<IRGBDevice> Devices { get; } = new();

    public OpenRGBClientWrapper(OpenRgbClient openRgbClient)
    {
        OpenRgbClient = openRgbClient;
    }

    public void Dispose()
    {
        OpenRgbClient.Dispose();
    }
}
