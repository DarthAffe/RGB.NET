using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for NodeMCU WS2812 devices.
/// </summary>
public class NodeMCUWS2812USBUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly string _hostname;

    private HttpClient _httpClient = new();
    private UdpClient? _udpClient;

    private readonly Dictionary<int, byte[]> _dataBuffer = new();
    private readonly Dictionary<int, byte> _sequenceNumbers = new();

    private readonly Action<byte[]> _sendDataAction;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeMCUWS2812USBUpdateQueue"/> class.
    /// If this constructor is used UDP updates are disabled.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="hostname">The hostname to connect to.</param>
    public NodeMCUWS2812USBUpdateQueue(IDeviceUpdateTrigger updateTrigger, string hostname)
        : base(updateTrigger)
    {
        this._hostname = hostname;

        _sendDataAction = SendHttp;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeMCUWS2812USBUpdateQueue"/> class.
    /// If this constructor is used UDP updates are enabled.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="hostname">The hostname to connect to.</param>
    /// <param name="udpPort">The port used by the UDP-connection.</param>
    public NodeMCUWS2812USBUpdateQueue(IDeviceUpdateTrigger updateTrigger, string hostname, int udpPort)
        : base(updateTrigger)
    {
        this._hostname = hostname;

        _udpClient = new UdpClient();
        EnableUdp(udpPort);

        _sendDataAction = SendUdp;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnStartup(object? sender, CustomUpdateData customData)
    {
        base.OnStartup(sender, customData);

        ResetDevice();
    }

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        foreach (IGrouping<int, ((int channel, int key), Color color)> channelData in dataSet.ToArray().Select(x => (((int channel, int key))x.key, x.color)).GroupBy(x => x.Item1.channel))
        {
            byte[] buffer = GetBuffer(channelData);
            _sendDataAction(buffer);
        }
    }

    private void SendHttp(byte[] buffer)
    {
        string data = Convert.ToBase64String(buffer);
        lock (_httpClient) _httpClient.PostAsync(GetUrl("update"), new StringContent(data, Encoding.ASCII)).Wait();
    }

    private void SendUdp(byte[] buffer)
    {
        _udpClient?.Send(buffer, buffer.Length);
    }

    private byte[] GetBuffer(IGrouping<int, ((int channel, int key) identifier, Color color)> data)
    {
        int channel = data.Key;
        byte[] buffer = _dataBuffer[channel];

        buffer[0] = GetSequenceNumber(channel);
        buffer[1] = (byte)channel;
        int i = 2;
        foreach ((byte _, byte r, byte g, byte b) in data.OrderBy(x => x.identifier.key)
                                                         .Select(x => x.color.GetRGBBytes()))
        {
            buffer[i++] = r;
            buffer[i++] = g;
            buffer[i++] = b;
        }

        return buffer;
    }

    internal IEnumerable<(int channel, int ledCount)> GetChannels()
    {
        string configString;
        lock (_httpClient) configString = _httpClient.GetStringAsync(GetUrl("config")).Result;

        configString = configString.Replace(" ", "")
                                   .Replace("\r", "")
                                   .Replace("\n", "");

        //HACK DarthAffe 13.07.2020: Adding a JSON-Parser dependency just for this is not really worth it right now ...
        MatchCollection channelMatches = Regex.Matches(configString, "\\{\"channel\":(?<channel>\\d+),\"leds\":(?<leds>\\d+)\\}");
        foreach (Match channelMatch in channelMatches)
        {
            int channel = int.Parse(channelMatch.Groups["channel"].Value);
            int leds = int.Parse(channelMatch.Groups["leds"].Value);
            if (leds > 0)
            {
                _dataBuffer[channel] = new byte[(leds * 3) + 2];
                _sequenceNumbers[channel] = 0;
                yield return (channel, leds);
            }
        }
    }

    internal void ResetDevice()
    {
        lock (_httpClient) _httpClient.GetStringAsync(GetUrl("reset")).Wait();
    }

    private void EnableUdp(int port)
    {
        _httpClient.PostAsync(GetUrl("enableUDP"), new StringContent(port.ToString(), Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Wait();
        _udpClient?.Connect(_hostname, port);
    }

    private byte GetSequenceNumber(int channel)
    {
        byte sequenceNumber = (byte)Math.Max(1, (_sequenceNumbers[channel] + 1) % byte.MaxValue);
        _sequenceNumbers[channel] = sequenceNumber;
        return sequenceNumber;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        lock (_httpClient)
        {
            base.Dispose();

            _udpClient?.Dispose();
            _udpClient = null;

            ResetDevice();
            _httpClient.Dispose();
        }
    }

    private string GetUrl(string path) => $"http://{_hostname}/{path}";

    #endregion
}