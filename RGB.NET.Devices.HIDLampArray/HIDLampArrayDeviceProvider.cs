// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;
using HidSharp;
using HidSharp.Reports;
using RGB.NET.Core;
using RGB.NET.HID;

namespace RGB.NET.Devices.HIDLampArray;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for PicoPi-devices.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class HIDLampArrayDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static HIDLampArrayDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="HIDLampArrayDeviceProvider"/> instance.
    /// </summary>
    public static HIDLampArrayDeviceProvider Instance => _instance ?? new HIDLampArrayDeviceProvider();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="HIDLampArrayDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public HIDLampArrayDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(HIDLampArrayDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    protected override void InitializeSDK()
    {
    }

    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach (HidDevice? device in DeviceList.Local.GetHidDevices())
        {

        }

        yield break;
    }

    #endregion
}