using System.Runtime.InteropServices;

namespace RGB.NET.Devices.CoolerMaster.Native;

// ReSharper disable once InconsistentNaming
internal struct _CoolerMasterColorMatrix
{
    #region Constants

    // ReSharper disable MemberCanBePrivate.Global

    internal const int ROWS = 8;
    internal const int COLUMNS = 24;

    // ReSharper restore MemberCanBePrivate.Global

    #endregion

    #region Properties & Fields

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = ROWS * COLUMNS)]
    public _CoolerMasterKeyColor[,] KeyColor;

    #endregion
}