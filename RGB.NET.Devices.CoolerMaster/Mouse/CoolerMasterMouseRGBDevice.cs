using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc cref="CoolerMasterRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a CoolerMaster mouse.
/// </summary>
public class CoolerMasterMouseRGBDevice : CoolerMasterRGBDevice<CoolerMasterMouseRGBDeviceInfo>, IMouse
{
    #region Constructors
        
    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CoolerMaster for the mouse</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal CoolerMasterMouseRGBDevice(CoolerMasterMouseRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        Dictionary<LedId, (int row, int column)> mapping = CoolerMasterMouseLedMappings.Mapping[DeviceInfo.DeviceIndex];

        foreach (KeyValuePair<LedId, (int row, int column)> led in mapping)
            AddLed(led.Key, new Point(led.Value.column * 19, led.Value.row * 19), new Size(19, 19));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => CoolerMasterMouseLedMappings.Mapping[DeviceInfo.DeviceIndex][ledId];

    #endregion
}