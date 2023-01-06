using System;

namespace RGB.NET.Devices.Novation.Attributes;

/// <inheritdoc />
/// <summary>
/// Specifies the led id mapping of a field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal class LedIdMappingAttribute : Attribute
{
    #region Properties & Fields

    /// <summary>
    /// Gets the led id mapping.
    /// </summary>
    internal LedIdMappings LedIdMapping { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.Attributes.LedIdMappingAttribute" /> class.
    /// </summary>
    /// <param name="ledIdMapping">The led id mapping.</param>
    internal LedIdMappingAttribute(LedIdMappings ledIdMapping)
    {
        this.LedIdMapping = ledIdMapping;
    }

    #endregion
}