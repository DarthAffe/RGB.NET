using System;
using System.Collections.Generic;
using OpenRGB.NET;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB;

internal sealed class OpenRGBClientWrapper : IDisposable
{
    public OpenRgbClient OpenRgbClient { get; }
    
    public IDeviceUpdateTrigger UpdateTrigger { get; }

    public List<IRGBDevice> Devices { get; } = new();

    public OpenRGBClientWrapper(OpenRgbClient openRgbClient, IDeviceUpdateTrigger updateTrigger)
    {
        OpenRgbClient = openRgbClient;
        UpdateTrigger = updateTrigger;
    }

    public void Dispose()
    {
        OpenRgbClient.Dispose();
        UpdateTrigger.Dispose();
    }
}
