using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace RGB.NET.Core.Layout
{
    /// <summary>
    /// Represents the serializable layout of a <see cref="Led"/>.
    /// </summary>
    [Serializable]
    [XmlType("Led")]
    public class LedLayout
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the Id of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

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
        /// Gets or sets the <see cref="RGB.NET.Core.Shape"/> of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public Shape Shape { get; set; }

        /// <summary>
        /// Gets or sets the vecor-data representing a custom-shape of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public string ShapeData { get; set; }

        /// <summary>
        /// Gets or sets the x-position of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public double X { get; private set; }

        /// <summary>
        /// Gets or sets the y-position of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public double Y { get; private set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public double Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlIgnore]
        public double Height { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the position- and size-data from the respective descriptive values.
        /// </summary>
        /// <param name="device">The <see cref="DeviceLayout"/> this <see cref="LedLayout"/> belongs to.</param>
        /// <param name="lastLed">The <see cref="LedLayout"/> previously calculated.</param>
        public void CalculateValues(DeviceLayout device, LedLayout lastLed)
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

        private double GetLocationValue(string value, double lastValue, double currentSize, double lastSize)
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
                    return lastValue + lastSize + double.Parse(value.Substring(1), CultureInfo.InvariantCulture);

                if (string.Equals(value, "-", StringComparison.Ordinal))
                    return lastValue - currentSize;

                if (value.StartsWith("-", StringComparison.Ordinal))
                    return lastValue - currentSize - double.Parse(value.Substring(1), CultureInfo.InvariantCulture);

                if (string.Equals(value, "~", StringComparison.Ordinal))
                    return (lastValue + lastSize) - currentSize;

                if (value.StartsWith("~", StringComparison.Ordinal))
                    return (lastValue + lastSize) - currentSize - double.Parse(value.Substring(1), CultureInfo.InvariantCulture);

                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        private double GetSizeValue(string value, double unitSize)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return 0;

                value = value.Replace(" ", string.Empty);

                if (value.EndsWith("mm", StringComparison.OrdinalIgnoreCase))
                    return double.Parse(value.Substring(0, value.Length - 2), CultureInfo.InvariantCulture);

                return unitSize * double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}
