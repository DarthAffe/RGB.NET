using System.Collections.Generic;
using HidSharp;

namespace RGB.NET.Devices.Logitech.HID
{
    //TODO DarthAffe 04.02.2017: Rewrite this once the SDK supports per-device lighting to get all the devices connected.
    internal static class DeviceChecker
    {
        #region Constants

        //TODO DarthAffe 04.02.2017: Add IDs
        private const int VENDOR_ID = 0x046D;
        private const int G910_ID = 0xC32B;
        private const int G810_ID = 0x0;

        #endregion

        #region Properties & Fields

        public static string ConnectedDeviceModel
        {
            get
            {
                if (IsG910Connected) return "G910";
                if (IsG810Connected) return "G810";
                return null;
            }
        }

        public static bool IsDeviceConnected => IsG910Connected || IsG810Connected;
        public static bool IsG910Connected { get; private set; }
        public static bool IsG810Connected { get; private set; }

        #endregion

        #region Methods

        internal static void LoadDeviceList()
        {
            IsG910Connected = false;
            IsG810Connected = false;

            HidDeviceLoader loader = new HidDeviceLoader();
            IEnumerable<HidDevice> devices = loader.GetDevices();
            foreach (HidDevice hidDevice in devices)
                if (hidDevice.VendorID == VENDOR_ID)
                {
                    if (hidDevice.ProductID == G910_ID)
                        IsG910Connected = true;
                    else if (hidDevice.ProductID == G810_ID)
                        IsG810Connected = true;
                }
        }

        #endregion
    }
}
