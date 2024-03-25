// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using Windows.System;
using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc cref="DynamicLightingRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Dynamic Lighting Keyboard-device.
/// </summary>
public sealed class DynamicLightingKeyboardRGBDevice : DynamicLightingRGBDevice<DynamicLightingKeyboardRGBDeviceInfo>, IKeyboard
{
    #region Properties & Fields

    /// <inheritdoc/>
    protected override LedId ReferenceLedId => LedId.Keyboard_Programmable1;

    IKeyboardDeviceInfo IKeyboard.DeviceInfo => DeviceInfo;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingKeyboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The meta data for this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal DynamicLightingKeyboardRGBDevice(DynamicLightingKeyboardRGBDeviceInfo info, DynamicLightingDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    /// <inheritdoc/>
    protected override LedMapping<int> CreateMapping()
    {
        LedMapping<int> ledMapping = [];
        HashSet<int> indices = Enumerable.Range(0, DeviceInfo.LedCount).ToHashSet();

        if (DeviceInfo.LampArray.SupportsVirtualKeys)
        {
            foreach ((VirtualKey virtualKey, LedId ledId) in LedMappings.KeyboardMapping)
            {
                int[] virtualKeyIndices = DeviceInfo.LampArray.GetIndicesForKey(virtualKey);
                if (virtualKeyIndices.Length > 0)
                {
                    int? index = virtualKeyIndices.FirstOrDefault(indices.Contains);
                    if (index != null)
                    {
                        ledMapping.Add(ledId, index.Value);
                        indices.Remove(index.Value);
                    }
                }
            }
        }

        LedId referenceLed = LedId.Keyboard_Custom1;
        foreach (int index in indices.OrderBy(x => x))
            ledMapping.Add(referenceLed++, index);

        return ledMapping;
    }

    #endregion
}