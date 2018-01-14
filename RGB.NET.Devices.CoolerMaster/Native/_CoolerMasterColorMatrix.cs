using System.Runtime.InteropServices;

namespace RGB.NET.Devices.CoolerMaster.Native
{
    // ReSharper disable once InconsistentNaming
    internal struct _CoolerMasterColorMatrix
    {
        #region Constants

        internal const int ROWS = 6;
        internal const int COLUMNS = 22;

        #endregion

        #region Properties & Fields

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ROWS * COLUMNS)]
        public _CoolerMasterKeyColor[,] KeyColor;

        #endregion
    }
}
