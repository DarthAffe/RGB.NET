using System;
using System.Reflection;
using RGB.NET.Devices.Novation.Attributes;

namespace RGB.NET.Devices.Novation;

/// <summary>
/// Offers some extensions and helper-methods for enum related things.
/// </summary>
internal static class EnumExtension
{
    /// <summary>
    /// Gets the value of the <see cref="DeviceIdAttribute"/>.
    /// </summary>
    /// <param name="source">The enum value to get the description from.</param>
    /// <returns>The value of the <see cref="DeviceIdAttribute"/> of the source.</returns>
    internal static string? GetDeviceId(this Enum source) => source.GetAttribute<DeviceIdAttribute>()?.Id;

    /// <summary>
    /// Gets the value of the <see cref="ColorCapabilityAttribute"/>.
    /// </summary>
    /// <param name="source">The enum value to get the description from.</param>
    /// <returns>The value of the <see cref="ColorCapabilityAttribute"/> of the source.</returns>
    internal static NovationColorCapabilities GetColorCapability(this Enum source) => source.GetAttribute<ColorCapabilityAttribute>()?.Capability ?? NovationColorCapabilities.None;

    /// <summary>
    /// Gets the value of the <see cref="LedIdMappingAttribute"/>.
    /// </summary>
    /// <param name="source">The enum value to get the description from.</param>
    /// <returns>The value of the <see cref="LedIdMappingAttribute"/> of the source.</returns>
    internal static LedIdMappings GetLedIdMapping(this Enum source) => source.GetAttribute<LedIdMappingAttribute>()?.LedIdMapping ?? LedIdMappings.Current;

    /// <summary>
    /// Gets the attribute of type T.
    /// </summary>
    /// <param name="source">The enum value to get the attribute from</param>
    /// <typeparam name="T">The generic attribute type</typeparam>
    /// <returns>The <see cref="Attribute"/>.</returns>
    private static T? GetAttribute<T>(this Enum source)
        where T : Attribute
    {
        FieldInfo? fi = source.GetType().GetField(source.ToString());
        if (fi == null) return null;
        T[] attributes = (T[])fi.GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? attributes[0] : null;
    }
}