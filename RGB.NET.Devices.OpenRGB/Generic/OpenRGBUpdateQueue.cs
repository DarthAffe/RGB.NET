using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Linq;
using Color = RGB.NET.Core.Color;
using OpenRGBColor = OpenRGB.NET.Color;
using OpenRGBDevice = OpenRGB.NET.Device;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for OpenRGB devices.
/// </summary>
public sealed class OpenRGBUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly int _deviceId;

    private readonly IOpenRgbClient _openRGB;
    private readonly OpenRGBColor[] _colors;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRGBUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceId">The index used to identify the device.</param>
    /// <param name="client">The OpenRGB client used to send updates to the OpenRGB server.</param>
    /// <param name="device">The OpenRGB Device containing device-specific information.</param>
    public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceId, IOpenRgbClient client, OpenRGBDevice device)
        : base(updateTrigger)
    {
        this._deviceId = deviceId;
        this._openRGB = client;

        _colors = Enumerable.Range(0, device.Colors.Length)
                            .Select(_ => new OpenRGBColor())
                            .ToArray();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            foreach ((object key, Color color) in dataSet)
                _colors[(int)key] = new OpenRGBColor(color.GetR(), color.GetG(), color.GetB());

            _openRGB.UpdateLeds(_deviceId, _colors);

            return true;
        }
        catch (Exception ex)
        {
            OpenRGBDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    #endregion
}
