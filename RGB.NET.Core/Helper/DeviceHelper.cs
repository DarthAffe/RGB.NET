using System.Reflection;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core;

/// <summary>
/// Offsers some helper methods for device creation.
/// </summary>
public static class DeviceHelper
{
    #region Methods

    /// <summary>
    /// Creates a unique device name from a manufacturer and model name.
    /// </summary>
    /// <remarks>
    /// The id is made unique based on the assembly calling this method.
    /// </remarks>
    /// <param name="manufacturer">The manufacturer of the device.</param>
    /// <param name="model">The model of the device.</param>
    /// <returns>The unique identifier for this device.</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string CreateDeviceName(string manufacturer, string model) => IdGenerator.MakeUnique(Assembly.GetCallingAssembly(), $"{manufacturer} {model}");

    #endregion
}