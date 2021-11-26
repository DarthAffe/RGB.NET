using System;

namespace RGB.NET.Devices.Novation.Attributes;

/// <inheritdoc />
/// <summary>
/// Specifies the device-id of a field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DeviceIdAttribute : Attribute
{
    #region Properties & Fields

    /// <summary>
    /// Gets the Id.
    /// </summary>
    public string Id { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.Attributes.DeviceIdAttribute" /> class.
    /// </summary>
    /// <param name="id">The id.</param>
    public DeviceIdAttribute(string id)
    {
        this.Id = id;
    }

    #endregion
}