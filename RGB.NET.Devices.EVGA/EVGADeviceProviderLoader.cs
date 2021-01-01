using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RGB.NET.Devices.EVGA
{
    public class EVGADeviceProviderLoader : IRGBDeviceProviderLoader
    {
        public static void Log(string msg)
        {
            try
            {
                File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
            }
            catch
            {
                System.Threading.Thread.Sleep(100);
                try
                {
                    File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
                }
                catch { }
            }
        }
        public EVGADeviceProviderLoader()
        {
            Log("loader constructor hit");
        }
        public bool RequiresInitialization => false;

        public IRGBDeviceProvider GetDeviceProvider()
        {
            return EVGADeviceProvider.Instance;
        }
    }
}
