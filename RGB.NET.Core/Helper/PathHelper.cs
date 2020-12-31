using System;
using System.IO;
using System.Reflection;

namespace RGB.NET.Core
{
    /// <summary>
    /// Offers some helper-methods for file-path related things.
    /// </summary>
    public static class PathHelper
    {
        #region Events

        /// <summary>
        /// Occurs when a path is resolving.
        /// </summary>
        public static event EventHandler<ResolvePathEventArgs>? ResolvingAbsolutePath;

        #endregion

        #region Methods

        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="relativePath">The relative part of the path to convert.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(string relativePath) => GetAbsolutePath((object?)null, relativePath);

        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="relativePath">The relative part of the path to convert.</param>
        /// <param name="fileName">The file name of the path to convert.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(string relativePath, string fileName) => GetAbsolutePath(null, relativePath, fileName);

        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="sender">The requester of this path. (Used for better control when using the event to override this behavior.)</param>
        /// <param name="relativePath">The relative path to convert.</param>
        /// <param name="fileName">The file name of the path to convert.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(object? sender, string relativePath, string fileName)
        {
            string relativePart = Path.Combine(relativePath, fileName);

            string? assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (assemblyLocation == null) return relativePart;

            string? directoryName = Path.GetDirectoryName(assemblyLocation);
            string path = directoryName == null ? string.Empty : Path.Combine(directoryName, relativePart);

            ResolvePathEventArgs args = new(relativePath, fileName, path);
            ResolvingAbsolutePath?.Invoke(sender, args);

            return args.FinalPath;
        }

        /// <summary>
        /// Returns an absolute path created from an relative path relatvie to the location of the executung assembly.
        /// </summary>
        /// <param name="sender">The requester of this path. (Used for better control when using the event to override this behavior.)</param>
        /// <param name="relativePath">The relative path to convert.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(object? sender, string relativePath)
        {
            string? assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (assemblyLocation == null) return relativePath;

            string? directoryName = Path.GetDirectoryName(assemblyLocation);
            string path = directoryName == null ? string.Empty : Path.Combine(directoryName, relativePath);

            ResolvePathEventArgs args = new(relativePath, path);
            ResolvingAbsolutePath?.Invoke(sender, args);

            return args.FinalPath;
        }

        #endregion
    }
}
