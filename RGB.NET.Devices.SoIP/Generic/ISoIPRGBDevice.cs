using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.SoIP.Generic
{
    // ReSharper disable once InconsistentNaming
    internal interface ISoIPRGBDevice : IRGBDevice, IDisposable
    {
        void Initialize(IDeviceUpdateTrigger updateTrigger);
    }
}
