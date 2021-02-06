using System;

namespace RGB.NET.Core
{
    public class ResolvePathEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the filename used to resolve the path.
        /// Also check <see cref="RelativePath "/> before use.
        /// </summary>
        public string? RelativePart { get; }

        /// <summary>
        /// Gets the filename used to resolve the path.
        /// Also check <see cref="RelativePath "/> before use.
        /// </summary>
        public string? FileName { get; }

        /// <summary>
        /// Gets the relative path used to resolve the path.
        /// If this is set <see cref="RelativePart" /> and <see cref="FileName" /> are unused.
        /// </summary>
        public string? RelativePath { get; }

        /// <summary>
        /// Gets or sets the resolved path.
        /// </summary>
        public string FinalPath { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Corer.ResolvePathEventArgs" /> class.
        /// </summary>
        /// <param name="relativePart">The filename used to resolve the path.</param>
        /// <param name="fileName">The filename used to resolve the path.</param>
        /// <param name="finalPath">The relative part used to resolve the path.</param>
        public ResolvePathEventArgs(string relativePart, string fileName, string finalPath)
        {
            this.RelativePart = relativePart;
            this.FileName = fileName;
            this.FinalPath = finalPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Corer.ResolvePathEventArgs" /> class.
        /// </summary>
        /// <param name="relativePath">The relative path used to resolve the path.</param>
        /// <param name="finalPath">The relative part used to resolve the path.</param>
        public ResolvePathEventArgs(string relativePath, string finalPath)
        {
            this.RelativePath = relativePath;
            this.FinalPath = finalPath;
        }

        #endregion
    }
}
