using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using RGB.NET.Core;

namespace RGB.NET.Layout
{
    /// <summary>
    /// Represents the serializable layout of a <see cref="IRGBDevice"/>.
    /// </summary>
    [Serializable]
    [XmlRoot("Device")]
    public class DeviceLayout : IDeviceLayout
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the name of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RGBDeviceType"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Type")]
        public RGBDeviceType Type { get; set; }

        /// <summary>
        /// Gets or sets the vendor of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Vendor")]
        public string? Vendor { get; set; }

        /// <summary>
        /// Gets or sets the model of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Model")]
        public string? Model { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Core.Shape"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Shape")]
        [DefaultValue(Shape.Rectangle)]
        public Shape Shape { get; set; } = Shape.Rectangle;

        /// <summary>
        /// Gets or sets the width of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width of one 'unit' used for the calculation of led positions and sizes.
        /// </summary>
        [XmlElement("LedUnitWidth")]
        [DefaultValue(19.0)]
        public double LedUnitWidth { get; set; } = 19.0;

        /// <summary>
        /// Gets or sets the height of one 'unit' used for the calculation of led positions and sizes.
        /// </summary>
        [XmlElement("LedUnitHeight")]
        [DefaultValue(19.0)]
        public double LedUnitHeight { get; set; } = 19.0;

        [XmlArray("Leds")]
        public List<LedLayout> InternalLeds { get; set; } = new();

        /// <summary>
        /// Gets or sets a list of <see cref="LedLayout"/> representing all the <see cref="Led"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<ILedLayout> Leds => InternalLeds;

        [XmlElement("CustomData")]
        public object? InternalCustomData { get; set; }

        [XmlIgnore]
        public object? CustomData { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <see cref="DeviceLayout"/> from the given xml.
        /// </summary>
        /// <param name="path">The path to the xml file.</param>
        /// <returns>The deserialized <see cref="DeviceLayout"/>.</returns>
        public static DeviceLayout? Load(string path, Type? customDeviceDataType = null, Type? customLedDataType = null)
        {
            if (!File.Exists(path)) return null;

            try
            {
                XmlSerializer serializer = new(typeof(DeviceLayout));
                using StreamReader reader = new(path);

                DeviceLayout? layout = serializer.Deserialize(reader) as DeviceLayout;
                if (layout != null)
                    layout.CustomData = layout.GetCustomData(layout.InternalCustomData, customDeviceDataType);

                if (layout?.InternalLeds != null)
                {
                    LedLayout? lastLed = null;
                    foreach (LedLayout led in layout.InternalLeds)
                    {
                        led.CalculateValues(layout, lastLed);
                        lastLed = led;

                        led.CustomData = layout.GetCustomData(led.InternalCustomData, customLedDataType);
                    }
                }

                return layout;
            }
            catch
            {
                return null;
            }
        }

        protected virtual object? GetCustomData(object? customData, Type? type)
        {
            XmlNode? node = (customData as XmlNode) ?? (customData as IEnumerable<XmlNode>)?.FirstOrDefault()?.ParentNode; //HACK DarthAffe 16.01.2021: This gives us the CustomData-Node
            if ((node == null) || (type == null)) return null;

            if (type == null) return null;

            using MemoryStream ms = new();
            using StreamWriter writer = new(ms);

            writer.Write(node.OuterXml);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            return new XmlSerializer(type).Deserialize(ms);
        }

        #endregion
    }
}
