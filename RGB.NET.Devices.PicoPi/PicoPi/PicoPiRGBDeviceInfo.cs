using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi
{
    public class PicoPiRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        public RGBDeviceType DeviceType { get; }
        public string DeviceName { get; }
        public string Manufacturer => "RGB.NET";
        public string Model { get; }
        public object? LayoutMetadata { get; set; }

        public int Id { get; }
        public int Version { get; }
        public int Channel { get; }
        public int LedCount { get; }

        #endregion

        #region Constructors

        internal PicoPiRGBDeviceInfo(RGBDeviceType deviceType, string model, int id, int version, int channel, int ledCount)
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.Id = id;
            this.Version = version;
            this.Channel = channel;
            this.LedCount = ledCount;

            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, $"{Model} {id} (Channel {channel})");
        }

        #endregion
    }
}
