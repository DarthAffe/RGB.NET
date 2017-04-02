using System.IO;
using System.Reflection;

namespace RGB.NET.Devices.Corsair.Helper
{
    /// <summary>
    /// Offers some helper-methods for file-path related things.
    /// </summary>
    internal static class PathHelper
    {
        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="relativePath">The relative path to convert.</param>
        /// <returns>The absolute path.</returns>
        internal static string GetAbsolutePath(string relativePath)
        {
            string assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (assemblyLocation == null) return relativePath;

            return Path.Combine(Path.GetDirectoryName(assemblyLocation), relativePath);
        }
    }
}
