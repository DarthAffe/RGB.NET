using System;
using System.ComponentModel;
using System.Reflection;
using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Helper
{
    /// <summary>
    /// Offers some extensions and helper-methods for enum related things.
    /// </summary>
    internal static class EnumExtension
    {
        /// <summary>
        /// Gets the value of the <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="source">The enum value to get the description from.</param>
        /// <typeparam name="T">The generic enum-type</typeparam>
        /// <returns>The value of the <see cref="DescriptionAttribute"/> or the <see cref="Enum.ToString()" /> result of the source.</returns>
        internal static string GetDescription<T>(this T source)
            where T : struct
        {
            return source.GetAttribute<DescriptionAttribute, T>()?.Description ?? source.ToString();
        }

        /// <summary>
        /// Gets the attribute of type T.
        /// </summary>
        /// <param name="source">The enum value to get the attribute from</param>
        /// <typeparam name="T">The generic attribute type</typeparam>
        /// <typeparam name="TEnum">The generic enum-type</typeparam>
        /// <returns>The <see cref="Attribute"/>.</returns>
        private static T GetAttribute<T, TEnum>(this TEnum source)
            where T : Attribute
            where TEnum : struct
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            T[] attributes = (T[])fi.GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
