namespace RGB.NET.Core
{
    /// <summary>
    /// Contains list of available update modes.
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// The <see cref="RGBSurface"/> will not perform automatic updates. Updates will only occur if <see cref="RGBSurface.Update" /> is called.
        /// </summary>
        Manual,

        /// <summary>
        /// The <see cref="RGBSurface"/> will perform automatic updates at the rate set in <see cref="RGBSurface.UpdateFrequency" />.
        /// </summary>
        Continuous
    }
}
