using System;
using System.Net.Sockets;
using RGB.NET.Core;

namespace RGB.NET.Devices.WLED;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for WLED devices.
/// </summary>
internal sealed class WledDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    /// <summary>
    /// The UDP-Connection used to send data.
    /// </summary>
    private readonly UdpClient _socket;

    /// <summary>
    /// The buffer the UDP-data is stored in.
    /// </summary>
    private byte[] _buffer;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WledDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceType">The device type used to identify the device.</param>
    public WledDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, string address, int port, int ledCount)
        : base(updateTrigger)
    {
        _buffer = new byte[2 + (ledCount * 3)];
        _buffer[0] = 2; // protocol: DRGB
        _buffer[1] = 2; // Timeout 2s

        _socket = new UdpClient();
        _socket.Connect(address, port);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnUpdate(object? sender, CustomUpdateData customData)
    {
        try
        {
            if (customData[CustomUpdateDataIndex.HEARTBEAT] as bool? ?? false)
                Update(Array.Empty<(object key, Color color)>());
            else
                base.OnUpdate(sender, customData);
        }
        catch (Exception ex)
        {
            WledDeviceProvider.Instance.Throw(ex);
        }
    }

    /// <inheritdoc />
    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            Span<byte> data = _buffer.AsSpan()[2..];
            foreach ((object key, Color color) in dataSet)
            {
                int ledIndex = (int)key;
                int offset = (ledIndex * 3);
                data[offset] = color.GetR();
                data[offset + 1] = color.GetG();
                data[offset + 2] = color.GetB();
            }

            _socket.Send(_buffer);

            return true;
        }
        catch (Exception ex)
        {
            WledDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        _socket.Dispose();
        _buffer = [];
    }

    #endregion
}