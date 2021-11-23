// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global

namespace RGB.NET.Devices.CoolerMaster.Native;

// ReSharper disable once InconsistentNaming
internal struct _CoolerMasterKeyColor
{
    #region Properties & Fields

    public byte R;
    public byte G;
    public byte B;

    #endregion

    #region Constructors

    internal _CoolerMasterKeyColor(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

    #endregion
}