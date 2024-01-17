[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for [WLED](https://kno.wled.ge/)-devices.

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many WLED-devices as you like
WledDeviceProvider.Instance.AddDeviceDefinition(new WledDeviceDefinition("<hostname>"));

surface.Load(WledDeviceProvider.Instance);
```

You can also override the manufacturer and device model in the DeviceDefinition.

# Required SDK
This provider does not require an additional SDK.

UDP realtime needs to be enabled on the WLED device.

# Automatic device discovery
Due to the requirement of an additional dependency and the requirement to be able to configure devices manually anywy automatic discovery is not part of the provider.   

Using the nuget `Tmds.MDns` you can use the following Helper to do this:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tmds.MDns;

namespace RGB.NET.Devices.WLED;

public static class WledDiscoveryHelper
{
    /// <summary>
    /// Searches for WLED devices and returns a list of devices found.
    /// </summary>
    /// <param name="waitFor">The time the discovery is waiting for responses. Choose this as short as possible as the method is blocking </param>
    /// <param name="maxDevices">The maximum amount of devices that are expected in the network. The discovery will stop early if the given amount of devices is found.</param>
    /// <returns>A list of addresses and device-infos.</returns>
    public static IEnumerable<(string address, WledInfo info)> DiscoverDevices(int waitFor = 500, int maxDevices = -1)
    {
        List<(string address, WledInfo info)> devices = [];
        using ManualResetEventSlim waitEvent = new();

        int devicesToDetect = maxDevices <= 0 ? int.MaxValue : maxDevices;

        ServiceBrowser mdns = new();
        mdns.ServiceAdded += OnServiceAdded;
        mdns.StartBrowse("_http._tcp");

        waitEvent.Wait(TimeSpan.FromMilliseconds(waitFor));

        mdns.StopBrowse();
        mdns.ServiceAdded -= OnServiceAdded;

        return devices;

        void OnServiceAdded(object? sender, ServiceAnnouncementEventArgs args)
        {
            string address = args.Announcement.Addresses.FirstOrDefault()?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(address))
            {
                WledInfo? info = null;
                try
                {
                    info = WledAPI.Info(address);
                }
                catch { }

                if (info != null)
                {
                    devices.Add((address, info));
                    if (--devicesToDetect <= 0)
                        waitEvent.Set();
                }
            }
        }
    }
}
```