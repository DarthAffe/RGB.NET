using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for NodeMCU WS2812 devices.
    /// </summary>
    public class NodeMCUWS2812USBUpdateQueue : UpdateQueue
    {
        #region Constants

        private static readonly byte UPDATE_COMMAND = 0x02;

        #endregion

        #region Properties & Fields

        private readonly string _hostname;

        private readonly UdpClient _udpClient;
        private readonly Dictionary<int, byte[]> _dataBuffer = new Dictionary<int, byte[]>();
        private readonly Dictionary<int, byte> _sequenceNumbers = new Dictionary<int, byte>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeMCUWS2812USBUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="hostname">The hostname to connect to.</param>
        /// <param name="port">The port used by the web-connection.</param>
        public NodeMCUWS2812USBUpdateQueue(IDeviceUpdateTrigger updateTrigger, string hostname, int port)
            : base(updateTrigger)
        {
            this._hostname = hostname;

            _udpClient = new UdpClient(_hostname, port);
            _udpClient.Connect(_hostname, port);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (IGrouping<int, ((int channel, int key), Color Value)> channelData in dataSet.Select(x => (((int channel, int key))x.Key, x.Value))
                                                                                                 .GroupBy(x => x.Item1.channel))
            {
                int channel = channelData.Key;
                byte[] buffer = _dataBuffer[channel];

                buffer[0] = GetSequenceNumber(channel);
                buffer[1] = (byte)((channel << 4) | UPDATE_COMMAND);
                int i = 2;
                foreach ((byte _, byte r, byte g, byte b) in channelData.OrderBy(x => x.Item1.key)
                                                                        .Select(x => x.Value.GetRGBBytes()))
                {
                    buffer[i++] = r;
                    buffer[i++] = g;
                    buffer[i++] = b;
                }

                Send(buffer);
            }
        }

        internal IEnumerable<(int channel, int ledCount)> GetChannels()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadString($"http://{_hostname}/reset");

            int channelCount = int.Parse(webClient.DownloadString($"http://{_hostname}/channels"));
            for (int channel = 1; channel <= channelCount; channel++)
            {
                int ledCount = int.Parse(webClient.DownloadString($"http://{_hostname}/channel/{channel}"));
                if (ledCount > 0)
                {
                    _dataBuffer[channel] = new byte[(ledCount * 3) + 2];
                    _sequenceNumbers[channel] = 0;
                    yield return (channel, ledCount);
                }
            }
        }

        private void Send(byte[] data) => _udpClient.Send(data, data.Length);

        private byte GetSequenceNumber(int channel)
        {
            byte sequenceNumber = (byte)((_sequenceNumbers[channel] + 1) % byte.MaxValue);
            _sequenceNumbers[channel] = sequenceNumber;
            return sequenceNumber;
        }

        #endregion
    }
}
