using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Logitech.HID;

[StructLayout(LayoutKind.Sequential, Pack = 0, Size = 7)]
internal struct FapShortRequest
{
    #region Constants

    private const byte LOGITECH_SHORT_MESSAGE = 0x10;

    #endregion

    #region Properties & Fields

    public byte ReportId;
    public byte DeviceIndex;
    public byte FeatureIndex;
    public byte FeatureCommand;
    public byte Data0;
    public byte Data1;
    public byte Data2;

    #endregion

    #region Constructors

    public void Init(byte deviceIndex, byte featureIndex)
    {
        this.DeviceIndex = deviceIndex;
        this.FeatureIndex = featureIndex;

        ReportId = LOGITECH_SHORT_MESSAGE;
        FeatureCommand = 0;
        Data0 = 0;
        Data1 = 0;
        Data2 = 0;
    }

    #endregion
}