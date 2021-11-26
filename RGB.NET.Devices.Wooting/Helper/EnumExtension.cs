using System;
using System.ComponentModel;
using System.Reflection;

namespace RGB.NET.Devices.Wooting.Helper;

/// <summary>
/// Offers some extensions and helper-methods for enum related things.
/// </summary>
internal static class EnumExtension
{
    /// <summary>
    /// Gets the value of the <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <param name="source">The enum value to get the description from.</param>
    /// <returns>The value of the <see cref="DescriptionAttribute"/> or the <see cref="System.Enum.ToString()" /> result of the source.</returns>
    internal static string GetDescription(this System.Enum source)
        => source.GetAttribute<DescriptionAttribute>()?.Description ?? source.ToString();
        
    /// <summary>
    /// Gets the attribute of type T.
    /// </summary>
    /// <param name="source">The enum value to get the attribute from</param>
    /// <typeparam name="T">The generic attribute type</typeparam>
    /// <returns>The <see cref="Attribute"/>.</returns>
    private static T? GetAttribute<T>(this System.Enum source)
        where T : Attribute
    {
        FieldInfo? fi = source.GetType().GetField(source.ToString());
        if (fi == null) return null;
        T[] attributes = (T[])fi.GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? attributes[0] : null;
    }
}