using System;
using System.Runtime.InteropServices;
using Google.Protobuf;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;
using WootingRgbSdk;

namespace RGB.NET.Devices.Wooting.Grpc;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for cooler master devices.
/// </summary>
public sealed class WootingGrpcUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly RgbSdkService.RgbSdkServiceClient _client;
    private readonly RgbGetConnectedDevicesResponse.Types.RgbDevice _wootDevice;
    private readonly WootingColor[] _colors;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WootingGrpcUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    public WootingGrpcUpdateQueue(IDeviceUpdateTrigger updateTrigger, RgbGetConnectedDevicesResponse.Types.RgbDevice wootDevice,
                                  RgbSdkService.RgbSdkServiceClient client)
        : base(updateTrigger)
    {
        this._client = client;
        this._wootDevice = wootDevice;
        this._colors = new WootingColor[WootingLedMappings.COLUMNS * WootingLedMappings.ROWS];
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            foreach ((object key, Color color) in dataSet)
            {
                (int row, int column) = ((int, int))key;
                int index = (WootingLedMappings.COLUMNS * row) + column;

                _colors[index] = new WootingColor(color.GetR(), color.GetG(), color.GetB());
            }

            _client.SetColors(new RgbSetColorsRequest
                              {
                                  Id = _wootDevice.Id,
                                  Colors = ByteString.CopyFrom(MemoryMarshal.AsBytes(_colors.AsSpan()))
                              });
            return true;
        }
        catch (Exception ex)
        {
            WootingGrpcDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _client.Close(new RgbCloseRequest { Id = _wootDevice.Id });
        base.Dispose();
    }

    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct WootingColor
{
    public readonly byte r;
    public readonly byte g;
    public readonly byte b;
    public readonly byte a;

    public WootingColor(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = 0; // Alpha is not used in Wooting devices
    }
}
