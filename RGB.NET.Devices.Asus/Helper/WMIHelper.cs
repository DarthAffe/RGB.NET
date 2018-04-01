#if NETFULL
using System.Management;

namespace RGB.NET.Devices.Asus
{
    // ReSharper disable once InconsistentNaming
    internal static class WMIHelper
    {
        #region Properties & Fields

        private static ManagementObjectSearcher _mainboardSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static (string manufacturer, string model)? _mainboardInfo;

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

        #endregion
    }
}
#endif
