// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.DMX.E131;

/// <summary>
/// Represents the data used to create a E1.31 DMX-device.
/// </summary>
public class E131DMXDeviceDefinition : IDMXDeviceDefinition
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets the hostname of the device.
    /// </summary>
    public string Hostname { get; set; }

    /// <summary>
    /// Gets or sets the port to device is listening to.
    /// </summary>
    public int Port { get; set; } = 5568;

    /// <summary>
    /// Gets or sets the <see cref="RGBDeviceType" /> of the device.
    /// </summary>
    public RGBDeviceType DeviceType { get; set; } = RGBDeviceType.Unknown;

    /// <summary>
    /// Gets or sets the manufacturer of the device.
    /// </summary>
    public string Manufacturer { get; set; } = "Unknown";

    /// <summary>
    /// Gets or sets the model name of the device.
    /// </summary>
    public string Model { get; set; } = "Generic DMX-Device";

    /// <summary>
    /// Gets or sets the CID of the device (null will generate a random CID)
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public byte[]? CID { get; set; }

    /// <summary>
    /// Gets or sets the universe the device belongs to.
    /// </summary>
    public short Universe { get; set; } = 1;

    /// <summary>
    /// Gets or sets the led-mappings used to create the device.
    /// </summary>
    public Dictionary<LedId, List<(int channel, Func<Color, byte> getValueFunc)>> Leds { get; } = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="E131DMXDeviceDefinition" /> class.
    /// </summary>
    /// <param name="hostname">The hostname of the device.</param>
    public E131DMXDeviceDefinition(string hostname)
    {
        this.Hostname = hostname;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a led-mapping to the device.
    /// </summary>
    /// <param name="id">The <see cref="LedId" /> used to identify the led.</param>
    /// <param name="channels">The channels the led is using and a function mapping parts of the color to them.</param>
    public void AddLed(LedId id, params (int channel, Func<Color, byte> getValueFunc)[] channels) => Leds[id] = channels.ToList();

    #endregion

    #region Factory

    //TODO DarthAffe 18.02.2018: Add factory-methods.

    #endregion
}