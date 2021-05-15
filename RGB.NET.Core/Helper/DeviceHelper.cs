using System.Reflection;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core
{
    public static class DeviceHelper
    {
        #region Methods

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string CreateDeviceName(string manufacturer, string model) => IdGenerator.MakeUnique(Assembly.GetCallingAssembly(), $"{manufacturer} {model}");

        #endregion
    }
}
