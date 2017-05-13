using System.IO;
using System.Reflection;

namespace RGB.NET.Core
{
    /// <summary>
    /// Offers some helper-methods for file-path related things.
    /// </summary>
    public static class PathHelper
    {
        #region Methods

        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="relativePath">The relative path to convert.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            string assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (assemblyLocation == null) return relativePath;

            string directoryName = Path.GetDirectoryName(assemblyLocation);
            return directoryName == null ? null : Path.Combine(directoryName, relativePath);
        }

        #endregion
    }
}
