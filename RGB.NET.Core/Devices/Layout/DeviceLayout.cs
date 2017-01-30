using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RGB.NET.Core.Layout
{
    /// <summary>
    /// Represents the serializable layout of a <see cref="IRGBDevice"/>.
    /// </summary>
    [Serializable]
    [XmlRoot("Device")]
    public class DeviceLayout
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the name of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RGBDeviceType"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Type")]
        public RGBDeviceType Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LayoutLighting"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Lighting")]
        public LayoutLighting Lighting { get; set; }

        /// <summary>
        /// Gets or sets the vendor of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Vendor")]
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or sets the model of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Model")]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LayoutShape"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Shape")]
        public LayoutShape Shape { get; set; }

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
        /// Gets or sets a list of <see cref="LedLayout"/> representing all the <see cref="Led"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlArray("Leds")]
        public List<LedLayout> Leds { get; set; } = new List<LedLayout>();

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <see cref="DeviceLayout"/> from the given xml.
        /// </summary>
        /// <param name="path">The path to the xml file.</param>
        /// <returns>The deserialized <see cref="DeviceLayout"/>.</returns>
        public static DeviceLayout Load(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DeviceLayout));
                using (StreamReader reader = new StreamReader(path))
                    return serializer.Deserialize(reader) as DeviceLayout;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
