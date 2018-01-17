using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Gets or sets the <see cref="RGBDeviceLighting"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlElement("Lighting")]
        public RGBDeviceLighting Lighting { get; set; }

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

        /// <summary>
        /// The path images for this device are collected in.
        /// </summary>
        [XmlElement("ImageBasePath")]
        public string ImageBasePath { get; set; }

        /// <summary>
        /// The image file for this device.
        /// </summary>
        [XmlElement("DeviceImage")]
        public string DeviceImage { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="LedLayout"/> representing all the <see cref="Led"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlArray("Leds")]
        public List<LedLayout> Leds { get; set; } = new List<LedLayout>();

        /// <summary>
        /// Gets or sets a list of <see cref="LedImageLayout"/> representing the layouts for the images of all the <see cref="Led"/> of the <see cref="DeviceLayout"/>.
        /// </summary>
        [XmlArray("LedImageLayouts")]
        public List<LedImageLayout> LedImageLayouts { get; set; } = new List<LedImageLayout>();

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
                {
                    DeviceLayout layout = serializer.Deserialize(reader) as DeviceLayout;
                    if (layout?.Leds != null)
                    {
                        LedLayout lastLed = null;
                        foreach (LedLayout led in layout.Leds)
                        {
                            led.CalculateValues(layout, lastLed);
                            lastLed = led;
                        }
                    }

                    return layout;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
