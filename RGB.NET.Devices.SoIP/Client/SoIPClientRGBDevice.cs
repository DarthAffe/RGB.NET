using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.SoIP.Generic;
using SimpleTCP;

namespace RGB.NET.Devices.SoIP.Client
{
    public class SoIPClientRGBDevice : AbstractRGBDevice<SoIPClientRGBDeviceInfo>, ISoIPRGBDevice
    {
        #region Properties & Fields

        private readonly Dictionary<LedId, Color> _syncbackCache = new Dictionary<LedId, Color>();
        private readonly SimpleTcpClient _tcpClient;

        public override SoIPClientRGBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        public SoIPClientRGBDevice(SoIPClientRGBDeviceInfo deviceInfo)
        {
            this.DeviceInfo = deviceInfo;

            _tcpClient = new SimpleTcpClient();
            _tcpClient.DelimiterDataReceived += TcpClientOnDelimiterDataReceived;
        }

        #endregion

        #region Methods

        void ISoIPRGBDevice.Initialize(IDeviceUpdateTrigger updateTrigger)
        {
            _tcpClient.Connect(DeviceInfo.Hostname, DeviceInfo.Port);
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        { }

        /// <inheritdoc />
        public override void SyncBack()
        {
            lock (_syncbackCache)
            {
                foreach (KeyValuePair<LedId, Color> cacheEntry in _syncbackCache)
                {
                    LedId ledId = cacheEntry.Key;
                    Color color = cacheEntry.Value;

                    if (!LedMapping.TryGetValue(ledId, out Led led))
                        led = InitializeLed(cacheEntry.Key, new Rectangle(0, 0, 10, 10)); //TODO DarthAffe 10.06.2018: Send layout with initial package

                    SetLedColorWithoutRequest(led, color);
                }

                _syncbackCache.Clear();
            }
        }

        private void TcpClientOnDelimiterDataReceived(object sender, Message message)
        {
            List<(LedId, Color)> leds = message.MessageString.Split(';').Select(x =>
                                                                                {
                                                                                    string[] led = x.Split('|');
                                                                                    return ((LedId)Enum.Parse(typeof(LedId), led[0]), RGBColor.FromHexString(led[1]));
                                                                                }).ToList();
            lock (_syncbackCache)
                foreach ((LedId ledId, Color color) in leds)
                    _syncbackCache[ledId] = color;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            _tcpClient.Disconnect();
            _tcpClient.Dispose();
        }

        #endregion
    }
}
