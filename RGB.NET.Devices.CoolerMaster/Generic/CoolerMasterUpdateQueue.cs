﻿using System;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for cooler master devices.
/// </summary>
public sealed class CoolerMasterUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly CoolerMasterDevicesIndexes _deviceIndex;
    private readonly _CoolerMasterColorMatrix _deviceMatrix;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CoolerMasterUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceIndex">The <see cref="CoolerMasterDevicesIndexes"/> of the device this queue performs updates for.</param>
    public CoolerMasterUpdateQueue(IDeviceUpdateTrigger updateTrigger, CoolerMasterDevicesIndexes deviceIndex)
        : base(updateTrigger)
    {
        this._deviceIndex = deviceIndex;

        _deviceMatrix = new _CoolerMasterColorMatrix { KeyColor = new _CoolerMasterKeyColor[_CoolerMasterColorMatrix.ROWS, _CoolerMasterColorMatrix.COLUMNS] };
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
                _deviceMatrix.KeyColor[row, column] = new _CoolerMasterKeyColor(color.GetR(), color.GetG(), color.GetB());
            }

            _CoolerMasterSDK.SetAllLedColor(_deviceMatrix, _deviceIndex);

            return true;
        }
        catch (Exception ex)
        {
            CoolerMasterDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    #endregion
}