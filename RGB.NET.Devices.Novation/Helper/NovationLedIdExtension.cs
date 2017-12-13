namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Offers some extensions and helper-methods for NovationLedId related things.
    /// </summary>
    public static class NovationLedIdExtension
    {
        #region Methods

        /// <summary>
        /// Gets the status-flag associated with the id.
        /// </summary>
        /// <param name="ledId">The <see cref="NovationLedId"/> whose status-flag should be determinated.</param>
        /// <returns>The status-flag of the <see cref="NovationLedId"/>.</returns>
        public static int GetStatus(this NovationLedId ledId) => ((int)ledId & 0xFF00) >> 8;

        /// <summary>
        /// Gets the id associated with the id.
        /// </summary>
        /// <param name="ledId">The <see cref="NovationLedId"/> whose idshould be determinated.</param>
        /// <returns>The id of the <see cref="NovationLedId"/>.</returns>
        public static int GetId(this NovationLedId ledId) => (int)ledId & 0x00FF;

        /// <summary>
        /// Tests if the given <see cref="NovationLedId"/> is a grid-button.
        /// </summary>
        /// <param name="ledId">the <see cref="NovationLedId"/> to test.</param>
        /// <returns><c>true</c> if <paramref name="ledId" /> is a grid-button; otherwise, <c>false</c>.</returns>
        public static bool IsGrid(this NovationLedId ledId) => (ledId.GetStatus() == 0x90) && ((ledId.GetId() / 0x10) < 0x08) && ((ledId.GetId() % 0x10) < 0x08);

        /// <summary>
        /// Tests if the given <see cref="NovationLedId"/> is a scene-button.
        /// </summary>
        /// <param name="ledId">the <see cref="NovationLedId"/> to test.</param>
        /// <returns><c>true</c> if <paramref name="ledId" /> is a scene-button; otherwise, <c>false</c>.</returns>
        public static bool IsScene(this NovationLedId ledId) => (ledId.GetStatus() == 0x90) && ((ledId.GetId() / 0x10) < 0x08) && ((ledId.GetId() % 0x10) == 0x09);

        /// <summary>
        /// Tests if the given <see cref="NovationLedId"/> is custom-button.
        /// </summary>
        /// <param name="ledId">the <see cref="NovationLedId"/> to test.</param>
        /// <returns><c>true</c> if <paramref name="ledId" /> is a custom-button; otherwise, <c>false</c>.</returns>
        public static bool IsCustom(this NovationLedId ledId) => (ledId.GetStatus() == 0xB0) && ((ledId.GetId() / 0x10) == 0x06) && ((ledId.GetId() % 0x10) > 0x07);

        #endregion
    }
}
