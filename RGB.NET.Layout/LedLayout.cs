using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;
using RGB.NET.Core;

namespace RGB.NET.Layout;

/// <summary>
/// Represents the serializable layout of a <see cref="Led"/>.
/// </summary>
[Serializable]
[XmlType("Led")]
public class LedLayout : ILedLayout
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets the Id of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlAttribute("Id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the descriptive <see cref="RGB.NET.Core.Shape"/> of the <see cref="LedLayout"/>.
    /// This property is for XML-serialization only and should not be directly accessed.
    /// </summary>
    [XmlElement("Shape")]
    [DefaultValue("Rectangle")]
    public string DescriptiveShape { get; set; } = "Rectangle";

    /// <summary>
    /// Gets or sets the descriptive x-position of the <see cref="LedLayout"/>.
    /// This property is for XML-serialization only and should not be directly accessed.
    /// </summary>
    [XmlElement("X")]
    [DefaultValue("+")]
    public string DescriptiveX { get; set; } = "+";

    /// <summary>
    /// Gets or sets the descriptive y-position of the <see cref="LedLayout"/>.
    /// This property is for XML-serialization only and should not be directly accessed.
    /// </summary>
    [XmlElement("Y")]
    [DefaultValue("=")]
    public string DescriptiveY { get; set; } = "=";

    /// <summary>
    /// Gets or sets the descriptive width of the <see cref="LedLayout"/>.
    /// This property is for XML-serialization only and should not be directly accessed.
    /// </summary>
    [XmlElement("Width")]
    [DefaultValue("1.0")]
    public string DescriptiveWidth { get; set; } = "1.0";

    /// <summary>
    /// Gets or sets the descriptive height of the <see cref="LedLayout"/>.
    /// This property is for XML-serialization only and should not be directly accessed.
    /// </summary>
    [XmlElement("Height")]
    [DefaultValue("1.0")]
    public string DescriptiveHeight { get; set; } = "1.0";

    /// <summary>
    /// Gets or sets the internal custom data of this layout.
    /// Normally you should use <see cref="CustomData"/> to access or set this data.
    /// </summary>
    [XmlElement("CustomData")]
    public object? InternalCustomData { get; set; }

    /// <inheritdoc />
    [XmlIgnore]
    public object? CustomData { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RGB.NET.Core.Shape"/> of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public Shape Shape { get; set; }

    /// <summary>
    /// Gets or sets the vecor-data representing a custom-shape of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public string? ShapeData { get; set; }

    /// <summary>
    /// Gets the x-position of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public float X { get; private set; }

    /// <summary>
    /// Gets the y-position of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public float Y { get; private set; }

    /// <summary>
    /// Gets the width of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public float Width { get; private set; }

    /// <summary>
    /// Gets the height of the <see cref="LedLayout"/>.
    /// </summary>
    [XmlIgnore]
    public float Height { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Calculates the position- and size-data from the respective descriptive values.
    /// </summary>
    /// <param name="device">The <see cref="DeviceLayout"/> this <see cref="LedLayout"/> belongs to.</param>
    /// <param name="lastLed">The <see cref="LedLayout"/> previously calculated.</param>
    public virtual void CalculateValues(DeviceLayout device, LedLayout? lastLed)
    {
        if (!Enum.TryParse(DescriptiveShape, true, out Shape shape))
        {
            shape = Shape.Custom;
            ShapeData = DescriptiveShape;
        }
        Shape = shape;

        Width = GetSizeValue(DescriptiveWidth, device.LedUnitWidth);
        Height = GetSizeValue(DescriptiveHeight, device.LedUnitHeight);

        X = GetLocationValue(DescriptiveX, lastLed?.X ?? 0, Width, lastLed?.Width ?? 0);
        Y = GetLocationValue(DescriptiveY, lastLed?.Y ?? 0, Height, lastLed?.Height ?? 0);
    }

    /// <summary>
    /// Gets the calculated location-value from the internal representation.
    /// </summary>
    /// <param name="value">The value provided by the layout.</param>
    /// <param name="lastValue">The location of the last calculated LED.</param>
    /// <param name="currentSize">The size of the current LED.</param>
    /// <param name="lastSize">The size of the last loaded LED.</param>
    /// <returns>The location-value of the LED.</returns>
    protected virtual float GetLocationValue(string value, float lastValue, float currentSize, float lastSize)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            value = value.Replace(" ", string.Empty);

            if (string.Equals(value, "=", StringComparison.Ordinal))
                return lastValue;

            if (string.Equals(value, "+", StringComparison.Ordinal))
                return lastValue + lastSize;

            if (value.StartsWith("+", StringComparison.Ordinal))
                return lastValue + lastSize + float.Parse(value[1..], CultureInfo.InvariantCulture);

            if (string.Equals(value, "-", StringComparison.Ordinal))
                return lastValue - currentSize;

            if (value.StartsWith("-", StringComparison.Ordinal))
                return lastValue - currentSize - float.Parse(value[1..], CultureInfo.InvariantCulture);

            if (string.Equals(value, "~", StringComparison.Ordinal))
                return (lastValue + lastSize) - currentSize;

            if (value.StartsWith("~", StringComparison.Ordinal))
                return (lastValue + lastSize) - currentSize - float.Parse(value[1..], CultureInfo.InvariantCulture);

            return float.Parse(value, CultureInfo.InvariantCulture);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Gets the calculated size-value from the internal representation.
    /// </summary>
    /// <param name="value">The value provided by the layout.</param>
    /// <param name="unitSize">The absolute size of one 'unit'.</param>
    /// <returns>The size-value of the LED.</returns>
    protected virtual float GetSizeValue(string value, float unitSize)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            value = value.Replace(" ", string.Empty);

            if (value.EndsWith("mm", StringComparison.OrdinalIgnoreCase))
                return float.Parse(value[..^2], CultureInfo.InvariantCulture);

            return unitSize * float.Parse(value, CultureInfo.InvariantCulture);
        }
        catch
        {
            return 0;
        }
    }

    #endregion
}