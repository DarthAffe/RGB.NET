using System;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries.API;
using RGB.NET.Devices.SteelSeries.Helper;

namespace RGB.NET.Devices.SteelSeries;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for steelseries devices.
/// </summary>
internal class SteelSeriesDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private string _deviceType;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SteelSeriesDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceType">The device type used to identify the device.</param>
    public SteelSeriesDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, string deviceType)
        : base(updateTrigger)
    {
        this._deviceType = deviceType;
    }

    #endregion

    #region Methods

    protected override void OnUpdate(object? sender, CustomUpdateData customData)
    {
        if (customData["refresh"] as bool? ?? false)
            SteelSeriesSDK.SendHeartbeat();
        else
            base.OnUpdate(sender, customData);
    }

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        => SteelSeriesSDK.UpdateLeds(_deviceType, dataSet.ToArray().Select(x => (((SteelSeriesLedId)x.key).GetAPIName(), x.color.ToIntArray())).Where(x => x.Item1 != null).ToList()!);

    #endregion
}