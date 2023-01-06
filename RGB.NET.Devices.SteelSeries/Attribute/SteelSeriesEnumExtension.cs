using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RGB.NET.Devices.SteelSeries;

internal static class SteelSeriesEnumExtension
{
    #region Properties & Fields
    // ReSharper disable InconsistentNaming

    private static readonly Dictionary<SteelSeriesDeviceType, string?> _deviceTypeNames = new();
    private static readonly Dictionary<SteelSeriesLedId, string?> _ledIdNames = new();

    // ReSharper restore InconsistentNaming
    #endregion

    #region Methods

    internal static string? GetAPIName(this SteelSeriesDeviceType deviceType)
    {
        if (!_deviceTypeNames.TryGetValue(deviceType, out string? apiName))
            _deviceTypeNames.Add(deviceType, apiName = GetAPIName(typeof(SteelSeriesDeviceType), deviceType));

        return apiName;
    }

    internal static string? GetAPIName(this SteelSeriesLedId ledId)
    {
        if (!_ledIdNames.TryGetValue(ledId, out string? apiName))
            _ledIdNames.Add(ledId, apiName = GetAPIName(typeof(SteelSeriesLedId), ledId));

        return apiName;
    }

    private static string? GetAPIName(Type type, Enum value)
    {
        MemberInfo[] memInfo = type.GetMember(value.ToString());
        if (memInfo.Length == 0) return null;
        return (memInfo.FirstOrDefault()?.GetCustomAttributes(typeof(APIName), false).FirstOrDefault() as APIName)?.Name;
    }

    #endregion
}