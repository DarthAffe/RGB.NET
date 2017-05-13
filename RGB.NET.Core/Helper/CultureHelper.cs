using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace RGB.NET.Core
{
    /// <summary>
    /// Offers some helper-methods for culture related things.
    /// </summary>
    public static class CultureHelper
    {
        #region DLLImports

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint thread);

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Gets the current keyboard-layout from the OS.
        /// </summary>
        /// <returns>The current keyboard-layout</returns>
        public static CultureInfo GetCurrentCulture()
        {
            try
            {
                int keyboardLayout = GetKeyboardLayout(0).ToInt32() & 0xFFFF;
                return new CultureInfo(keyboardLayout);
            }
            catch
            {
                return new CultureInfo(1033); // en-US on error.
            }
        }

        #endregion
    }
}
