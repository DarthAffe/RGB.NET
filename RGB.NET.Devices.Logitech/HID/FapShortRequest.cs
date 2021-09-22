using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Logitech.HID
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = 7)]
    public struct FapShortRequest
    {
        const byte LOGITECH_SHORT_MESSAGE = 0x10;

        public byte ReportId;
        public byte DeviceIndex;
        public byte FeatureIndex;
        public byte FeatureCommand;
        public byte Data0;
        public byte Data1;
        public byte Data2;

        public void Init(byte deviceIndex, byte featureIndex)
        {
            ReportId = LOGITECH_SHORT_MESSAGE;
            DeviceIndex = deviceIndex;
            FeatureIndex = featureIndex;
            FeatureCommand = 0;
            Data0 = 0;
            Data1 = 0;
            Data2 = 0;
        }
    }
}
