using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Linq;
using OpenRGBColor = OpenRGB.NET.Models.Color;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for OpenRGB devices.
/// </summary>
public class OpenRGBUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly int _deviceid;

    private readonly OpenRGBClient _openRGB;
    private readonly OpenRGBDevice _device;
    private readonly OpenRGBColor[] _colors;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRGBUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceid">The index used to identify the device.</param>
    /// <param name="client">The OpenRGB client used to send updates to the OpenRGB server.</param>
    /// <param name="device">The OpenRGB Device containing device-specific information.</param>
    public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, OpenRGBDevice device)
        : base(updateTrigger)
    {
        this._deviceid = deviceid;
        this._openRGB = client;
        this._device = device;

        _colors = Enumerable.Range(0, _device.Colors.Length)
                            .Select(_ => new OpenRGBColor())
                            .ToArray();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        foreach ((object key, Color color) in dataSet)
            _colors[(int)key] = new OpenRGBColor(color.GetR(), color.GetG(), color.GetB());

        _openRGB.UpdateLeds(_deviceid, _colors);
    }

    #endregion
}
