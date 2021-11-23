using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HidSharp;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi;

/// <summary>
/// Represents a SDK to access devices based on a Raspberry Pi Pico.
/// </summary>
public class PicoPiSDK : IDisposable
{
    #region Constants

    /// <summary>
    /// The vendor id used by the pico-pi firmware.
    /// Registered at https://pid.codes/1209/2812/
    /// </summary>
    public const int VENDOR_ID = 0x1209;

    /// <summary>
    /// The product id used by the pico-pi firmware.
    /// Registered at https://pid.codes/1209/2812/
    /// </summary>
    public const int HID_BULK_CONTROLLER_PID = 0x2812;

    private const byte COMMAND_CHANNEL_COUNT = 0x01;
    private const byte COMMAND_LEDCOUNTS = 0x0A;
    private const byte COMMAND_PINS = 0x0B;
    private const byte COMMAND_ID = 0x0E;
    private const byte COMMAND_VERSION = 0x0F;
    private const byte COMMAND_UPDATE = 0x01;
    private const byte COMMAND_UPDATE_BULK = 0x02;

    #endregion

    #region Properties & Fields

    private readonly HidDevice _hidDevice;
    private readonly HidStream _hidStream;

    private UsbContext? _usbContext;
    private IUsbDevice? _bulkDevice;
    private UsbEndpointWriter? _bulkWriter;

    private readonly byte[] _hidSendBuffer;
    private readonly byte[] _bulkSendBuffer;

    private int _bulkTransferLength;

    /// <summary>
    /// Gets if updates via the Bulk-Enbpoint are possible.
    /// </summary>
    public bool IsBulkSupported { get; private set; }

    /// <summary>
    /// Gets the Id of the device.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the version of the device firmware.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets a collection of channels, led counts and pins that are available on this device.
    /// </summary>
    public IReadOnlyList<(int channel, int ledCount, int pin)> Channels { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PicoPiSDK" /> class.
    /// </summary>
    /// <param name="device">The underlying hid device.</param>
    public PicoPiSDK(HidDevice device)
    {
        this._hidDevice = device;

        _hidSendBuffer = new byte[_hidDevice.GetMaxOutputReportLength() - 1];

        _hidStream = _hidDevice.Open();
        LoadBulkDevice();

        Id = GetId();
        Version = GetVersion();
        Channels = new ReadOnlyCollection<(int channel, int ledCount, int pin)>(GetChannels().ToList());

        _bulkSendBuffer = new byte[(Channels.Sum(c => c.ledCount + 1) * 3) + 5];
    }

    #endregion

    #region Methods

    /// <summary>
    /// Configures the amount of leds to update on the specified channels.
    /// </summary>
    /// <remarks>This is a configuration function. The value is persistent on the device.</remarks>
    /// <param name="ledCounts">The values to set on the device.</param>
    public void SetLedCounts(params (int channel, int ledCount)[] ledCounts)
    {
        byte[] data = new byte[Channels.Count + 2];
        data[1] = COMMAND_LEDCOUNTS;
        foreach ((int channel, int ledCount, _) in Channels)
            data[channel + 1] = (byte)ledCount;

        foreach ((int channel, int ledCount) in ledCounts)
            data[channel + 1] = (byte)ledCount;

        SendHID(data);
    }

    /// <summary>
    /// Configures the pins used by the specified channels.
    /// </summary>
    /// <remarks>This is a configuration function. The value is persistent on the device.</remarks>
    /// <param name="pins">T values to set on the device.</param>
    public void SetPins(params (int channel, int pin)[] pins)
    {
        byte[] data = new byte[Channels.Count + 2];
        data[1] = COMMAND_PINS;
        foreach ((int channel, _, int pin) in Channels)
            data[channel + 1] = (byte)pin;

        foreach ((int channel, int pin) in pins)
            data[channel + 1] = (byte)pin;

        SendHID(data);
    }

    private void LoadBulkDevice()
    {
        try
        {
            _usbContext = new UsbContext();
            // DarthAffe 24.04.2021: Not using .Find as it's not returning the device :(
            IEnumerable<IUsbDevice> devices = _usbContext.List().Where(d => (d.VendorId == _hidDevice.VendorID) && (d.ProductId == _hidDevice.ProductID));
            foreach (IUsbDevice device in devices)
            {
                try
                {
                    device.Open();
                    if (device.Info.SerialNumber == _hidDevice.GetSerialNumber())
                    {
                        _bulkDevice = device;
                        break;
                    }
                    device.Dispose();
                }
                catch { /**/ }
            }

            if (_bulkDevice != null)
            {
                _bulkDevice.ClaimInterface(1);
                _bulkWriter = _bulkDevice.OpenEndpointWriter(WriteEndpointID.Ep02, EndpointType.Bulk);

                IsBulkSupported = true;
            }
        }
        catch
        {
            _bulkWriter = null;
            try { _bulkDevice?.Dispose(); } catch { /**/ }
            try { _usbContext?.Dispose(); } catch { /**/ }
            _bulkDevice = null;
        }
    }

    private string GetId()
    {
        SendHID(0x00, COMMAND_ID);
        return ConversionHelper.ToHex(Read().Skip(1).Take(8).ToArray());
    }

    private int GetVersion()
    {
        SendHID(0x00, COMMAND_VERSION);
        return Read()[1];
    }

    private IEnumerable<(int channel, int ledCount, int pin)> GetChannels()
    {
        SendHID(0x00, COMMAND_CHANNEL_COUNT);
        int channelCount = Read()[1];

        for (int i = 1; i <= channelCount; i++)
        {
            SendHID(0x00, (byte)((i << 4) | COMMAND_LEDCOUNTS));
            int ledCount = Read()[1];

            SendHID(0x00, (byte)((i << 4) | COMMAND_PINS));
            int pin = Read()[1];

            yield return (i, ledCount, pin);
        }
    }

    /// <summary>
    /// Sends a update to the device using the HID-endpoint.
    /// </summary>
    /// <param name="data">The data packet to send.</param>
    /// <param name="channel">The channel to update.</param>
    /// <param name="chunk">The chunk id of the packet. (Required if packets are fragmented.)</param>
    /// <param name="update">A value indicating if the device should update directly after receiving this packet. (If packets are fragmented this should only be true for the last chunk.)</param>
    public void SendHidUpdate(in Span<byte> data, int channel, int chunk, bool update)
    {
        if (data.Length == 0) return;

        Span<byte> sendBuffer = _hidSendBuffer;
        sendBuffer[0] = 0x00;
        sendBuffer[1] = (byte)((channel << 4) | COMMAND_UPDATE);
        sendBuffer[2] = update ? (byte)1 : (byte)0;
        sendBuffer[3] = (byte)chunk;
        data.CopyTo(sendBuffer.Slice(4, data.Length));
        SendHID(_hidSendBuffer);
    }

    /// <summary>
    /// Sends a update to the device using the bulk-endpoint.
    /// </summary>
    /// <remarks>
    /// Silently fails if not bulk-updates are supported. (Check <see cref="IsBulkSupported"/>)
    /// </remarks>
    /// <param name="data">The data packet to send.</param>
    /// <param name="channel">The channel to update.</param>
    public void SendBulkUpdate(in Span<byte> data, int channel)
    {
        if ((data.Length == 0) || !IsBulkSupported) return;

        Span<byte> sendBuffer = new Span<byte>(_bulkSendBuffer)[2..];
        int payloadSize = data.Length;

        sendBuffer[_bulkTransferLength++] = (byte)((channel << 4) | COMMAND_UPDATE_BULK);
        sendBuffer[_bulkTransferLength++] = (byte)((payloadSize >> 8) & 0xFF);
        sendBuffer[_bulkTransferLength++] = (byte)(payloadSize & 0xFF);
        data.CopyTo(sendBuffer.Slice(_bulkTransferLength, payloadSize));
        _bulkTransferLength += payloadSize;
    }

    /// <summary>
    /// Flushing the bulk endpoint, causing the device to update.
    /// </summary>
    public void FlushBulk()
    {
        if (_bulkTransferLength == 0) return;

        _bulkSendBuffer[0] = (byte)((_bulkTransferLength >> 8) & 0xFF);
        _bulkSendBuffer[1] = (byte)(_bulkTransferLength & 0xFF);
        SendBulk(_bulkSendBuffer, _bulkTransferLength + 2);

        _bulkTransferLength = 0;
    }

    private void SendHID(params byte[] data) => _hidStream.Write(data);
    private void SendBulk(byte[] data, int count) => _bulkWriter!.Write(data, 0, count, 1000, out int _);

    private byte[] Read() => _hidStream.Read();

    /// <inheritdoc />
    public void Dispose()
    {
        _hidStream.Dispose();
        _bulkDevice?.Dispose();
        _usbContext?.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion
}