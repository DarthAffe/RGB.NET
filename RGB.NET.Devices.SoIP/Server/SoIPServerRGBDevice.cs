using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using RGB.NET.Core;
using RGB.NET.Devices.SoIP.Generic;
using SimpleTCP;

namespace RGB.NET.Devices.SoIP.Server
{
    public class SoIPServerRGBDevice : AbstractRGBDevice<SoIPServerRGBDeviceInfo>, ISoIPRGBDevice
    {
        #region Properties & Fields

        private readonly List<LedId> _leds;
        private readonly SimpleTcpServer _tcpServer;
        private SoIPServerUpdateQueue _updateQueue;

        public override SoIPServerRGBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        public SoIPServerRGBDevice(SoIPServerRGBDeviceInfo deviceInfo, List<LedId> leds)
        {
            this.DeviceInfo = deviceInfo;
            this._leds = leds;

            _tcpServer = new SimpleTcpServer();
            _tcpServer.ClientConnected += TcpServerOnClientConnected;
        }

        #endregion

        #region Methods

        void ISoIPRGBDevice.Initialize(IDeviceUpdateTrigger updateTrigger)
        {
            int count = 0;
            foreach (LedId id in _leds)
                InitializeLed(id, new Rectangle((count++) * 10, 0, 10, 10));

            //TODO DarthAffe 10.06.2018: Allow to load a layout.

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }

            _tcpServer.Start(DeviceInfo.Port);
            _updateQueue = new SoIPServerUpdateQueue(updateTrigger, _tcpServer);
        }

        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => _updateQueue.SetData(ledsToUpdate);

        private void TcpServerOnClientConnected(object sender, TcpClient tcpClient)
        {
            string message = GetLedString(LedMapping.Values);
            byte[] messageData = _tcpServer.StringEncoder.GetBytes(message + _tcpServer.StringEncoder.GetString(new[] { _tcpServer.Delimiter }));
            tcpClient.GetStream().WriteAsync(messageData, 0, messageData.Length);
        }

        private string GetLedString(IEnumerable<Led> leds) => string.Join(";", leds.Select(x => x.Id.ToString() + "|" + x.Color.AsARGBHexString(false)));

        /// <inheritdoc />
        public override void Dispose()
        {
            try { _updateQueue?.Dispose(); }
            catch { /* at least we tried */ }

            base.Dispose();

            _tcpServer.Stop();
        }

        #endregion
    }
}
