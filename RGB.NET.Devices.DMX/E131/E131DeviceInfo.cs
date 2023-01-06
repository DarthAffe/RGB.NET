using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.DMX.E131;

/// <inheritdoc />
/// <summary>
/// Represents device information for a <see cref="E131Device"/> />.
/// </summary>
public class E131DeviceInfo : IRGBDeviceInfo
{
    #region Constants

    /// <summary>
    /// The length of the CID;
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public const int CID_LENGTH = 16;

    #endregion

    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer { get; }

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// The hostname of the device.
    /// </summary>
    public string Hostname { get; }

    /// <summary>
    /// The port of the device. 
    /// </summary>
    public int Port { get; }

    /// <summary>
    /// The CID used to identify against the device.
    /// </summary>
    public byte[] CID { get; }

    /// <summary>
    /// The Universe this device belongs to.
    /// </summary>
    public short Universe { get; }

    #endregion

    #region Constructors

    internal E131DeviceInfo(E131DMXDeviceDefinition deviceDefinition)
    {
        this.DeviceType = deviceDefinition.DeviceType;
        this.Manufacturer = deviceDefinition.Manufacturer;
        this.Model = deviceDefinition.Model;
        this.Hostname = deviceDefinition.Hostname;
        this.Port = deviceDefinition.Port;
        this.Universe = deviceDefinition.Universe;

        byte[]? cid = deviceDefinition.CID;
        if ((cid == null) || (cid.Length != CID_LENGTH))
        {
            cid = new byte[CID_LENGTH];
            new Random().NextBytes(cid);
        }

        CID = cid;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}