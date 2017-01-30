namespace RGB.NET.Core.Layout
{
    /// <summary>
    /// Contains a list of different lightning-modes used by <see cref="DeviceLayout"/>.
    /// </summary>
    public enum LayoutLighting
    {
        /// <summary>
        /// The <see cref="IRGBDevice"/> supports per-key-lightning.
        /// </summary>
        Key = 0,

        /// <summary>
        /// The <see cref="IRGBDevice"/> supports per-keyboard-lightning.
        /// </summary>
        Keyboard = 1,
    }
}
