using System;
using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for MSI devices.
/// </summary>
public class MsiDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private string _deviceType;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MsiDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceType">The device-type used to identify the device.</param>
    public MsiDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, string deviceType)
        : base(updateTrigger)
    {
        this._deviceType = deviceType;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        foreach ((object key, Color color) in dataSet)
            _MsiSDK.SetLedColor(_deviceType, (int)key, color.GetR(), color.GetG(), color.GetB());
    }

    #endregion
}