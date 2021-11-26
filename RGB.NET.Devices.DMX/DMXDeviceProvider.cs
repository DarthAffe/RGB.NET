// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.DMX.E131;

namespace RGB.NET.Devices.DMX;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for DMX devices.
/// </summary>
public class DMXDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static DMXDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="DMXDeviceProvider"/> instance.
    /// </summary>
    public static DMXDeviceProvider Instance => _instance ?? new DMXDeviceProvider();

    /// <summary>
    /// Gets a list of all defined device-definitions.
    /// </summary>
    public List<IDMXDeviceDefinition> DeviceDefinitions { get; } = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DMXDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public DMXDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(DMXDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified <see cref="IDMXDeviceDefinition" /> to this device-provider.
    /// </summary>
    /// <param name="deviceDefinition">The <see cref="IDMXDeviceDefinition"/> to add.</param>
    public void AddDeviceDefinition(IDMXDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach (IDMXDeviceDefinition dmxDeviceDefinition in DeviceDefinitions)
        {
            IRGBDevice? device = null;
            try
            {
                if (dmxDeviceDefinition is E131DMXDeviceDefinition e131DMXDeviceDefinition)
                    if (e131DMXDeviceDefinition.Leds.Count > 0)
                        device = new E131Device(new E131DeviceInfo(e131DMXDeviceDefinition), e131DMXDeviceDefinition.Leds, GetUpdateTrigger(0));
            }
            catch (Exception ex)
            {
                Throw(ex);
            }

            if (device != null)
                yield return device;
        }
    }

    #endregion
}