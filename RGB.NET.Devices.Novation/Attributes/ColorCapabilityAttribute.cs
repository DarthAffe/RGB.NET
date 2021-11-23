using System;

namespace RGB.NET.Devices.Novation.Attributes;

/// <inheritdoc />
/// <summary>
/// Specifies the color-capability of a field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ColorCapabilityAttribute : Attribute
{
    #region Properties & Fields

    /// <summary>
    /// Gets the Id.
    /// </summary>
    public NovationColorCapabilities Capability { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.Attributes.ColorCapabilityAttribute" /> class.
    /// </summary>
    /// <param name="capability">The capability.</param>
    public ColorCapabilityAttribute(NovationColorCapabilities capability)
    {
        this.Capability = capability;
    }

    #endregion
}