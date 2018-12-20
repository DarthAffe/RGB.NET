#if NETFULL
using System.Management;

namespace RGB.NET.Devices.Asus
{
    // ReSharper disable once InconsistentNaming
    internal static class WMIHelper
    {
        #region Properties & Fields

        private static ManagementObjectSearcher _mainboardSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT Manufacturer,Product FROM Win32_BaseBoard");
        private static ManagementObjectSearcher _graphicsCardSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT Name FROM Win32_VideoController");

        private static (string manufacturer, string model)? _mainboardInfo;
        private static string _graphicsCardInfo;

        #endregion

        #region Methods

        internal static (string manufacturer, string model)? GetMainboardInfo()
        {
            if (!_mainboardInfo.HasValue)
                foreach (ManagementBaseObject managementBaseObject in _mainboardSearcher.Get())
                {
                    _mainboardInfo = (managementBaseObject["Manufacturer"]?.ToString(), managementBaseObject["Product"]?.ToString());
                    break;
                }

            return _mainboardInfo;
        }

        internal static string GetGraphicsCardsInfo()
        {
            if (_graphicsCardInfo == null)
                foreach (ManagementBaseObject managementBaseObject in _graphicsCardSearcher.Get())
                {
                    _graphicsCardInfo = managementBaseObject["Name"]?.ToString();
                    break;
                }

            return _graphicsCardInfo;
        }

        #endregion
    }
}
#else
namespace RGB.NET.Devices.Asus
{
    // ReSharper disable once InconsistentNaming
    internal static class WMIHelper
    {
        #region Methods

        internal static (string manufacturer, string model)? GetMainboardInfo() => null;

        internal static string GetGraphicsCardsInfo() => null;

        #endregion
    }
}
#endif
