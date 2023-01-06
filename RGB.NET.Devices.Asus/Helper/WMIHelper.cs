using System;
using System.Management;

namespace RGB.NET.Devices.Asus;

// ReSharper disable once InconsistentNaming
internal static class WMIHelper
{
    #region Properties & Fields

    // ReSharper disable InconsistentNaming
    private static readonly ManagementObjectSearcher? _systemModelSearcher;
    private static readonly ManagementObjectSearcher? _mainboardSearcher;
    private static readonly ManagementObjectSearcher? _graphicsCardSearcher;
    // ReSharper restore InconsistentNaming

    private static string? _systemModelInfo;
    private static (string manufacturer, string model)? _mainboardInfo;
    private static string? _graphicsCardInfo;
        
    #endregion

    #region Constructors

    static WMIHelper()
    {
        if (OperatingSystem.IsWindows())
        {
            _systemModelSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT Model FROM Win32_ComputerSystem");
            _mainboardSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT Manufacturer,Product FROM Win32_BaseBoard");
            _graphicsCardSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT Name FROM Win32_VideoController");
        }
    }

    #endregion

    #region Methods

    internal static string? GetSystemModelInfo()
    {
        if (!OperatingSystem.IsWindows()) return null;

        if ((_systemModelInfo == null) && (_systemModelSearcher != null))
            foreach (ManagementBaseObject managementBaseObject in _systemModelSearcher.Get())
            {
                _systemModelInfo = managementBaseObject["Model"]?.ToString();
                break;
            }

        return _systemModelInfo;
    }

    internal static (string manufacturer, string model)? GetMainboardInfo()
    {
        if (!OperatingSystem.IsWindows()) return null;

        if (!_mainboardInfo.HasValue && (_mainboardSearcher != null))
            foreach (ManagementBaseObject managementBaseObject in _mainboardSearcher.Get())
            {
                _mainboardInfo = (managementBaseObject["Manufacturer"]?.ToString() ?? string.Empty, managementBaseObject["Product"]?.ToString() ?? string.Empty);
                break;
            }

        return _mainboardInfo;
    }

    internal static string? GetGraphicsCardsInfo()
    {
        if (!OperatingSystem.IsWindows()) return null;

        if ((_graphicsCardInfo == null) && (_graphicsCardSearcher != null))
            foreach (ManagementBaseObject managementBaseObject in _graphicsCardSearcher.Get())
            {
                _graphicsCardInfo = managementBaseObject["Name"]?.ToString();
                break;
            }

        return _graphicsCardInfo;
    }

    #endregion
}