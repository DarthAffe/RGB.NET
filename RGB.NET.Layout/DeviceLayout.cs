using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using RGB.NET.Core;

namespace RGB.NET.Layout;

/// <summary>
/// Represents the serializable layout of a <see cref="IRGBDevice"/>.
/// </summary>
[Serializable]
[XmlRoot("Device")]
public class DeviceLayout : DeviceLayout<object, object>
{ }

/// <summary>
/// Represents the serializable layout of a <see cref="IRGBDevice"/>.
/// </summary>
[Serializable]
[XmlRoot("Device")]
public class DeviceLayout<TCustomData> : DeviceLayout<TCustomData, object>
{ }

/// <summary>
/// Represents the serializable layout of a <see cref="IRGBDevice"/>.
/// </summary>
[Serializable]
[XmlRoot("Device")]
public class DeviceLayout<TCustomData, TCustomLedData> : IDeviceLayout
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
    /// Gets or sets the author of the <see cref="DeviceLayout"/>.
    /// </summary>
    [XmlElement("Author")]
    public string? Author { get; set; }

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
    public float Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the <see cref="DeviceLayout"/>.
    /// </summary>
    [XmlElement("Height")]
    public float Height { get; set; }

    /// <summary>
    /// Gets or sets the width of one 'unit' used for the calculation of led positions and sizes.
    /// </summary>
    [XmlElement("LedUnitWidth")]
    [DefaultValue(19.0)]
    public float LedUnitWidth { get; set; } = 19.0f;

    /// <summary>
    /// Gets or sets the height of one 'unit' used for the calculation of led positions and sizes.
    /// </summary>
    [XmlElement("LedUnitHeight")]
    [DefaultValue(19.0)]
    public float LedUnitHeight { get; set; } = 19.0f;

    /// <summary>
    /// Gets or sets the internal list of led layouts.
    /// Normally you should use <see cref="Leds"/> to access this data.
    /// </summary>
    [XmlArray("Leds")]
    public List<LedLayout<TCustomLedData>> InternalLeds { get; set; } = new();

    /// <summary>
    /// Gets or sets a list of <see cref="LedLayout"/> representing all the <see cref="Led"/> of the <see cref="DeviceLayout"/>.
    /// </summary>
    [XmlIgnore]
    public IEnumerable<ILedLayout> Leds => InternalLeds;

    /// <summary>
    /// Gets or sets the custom data of this layout.
    /// </summary>
    [XmlElement("CustomData")]
    public TCustomData? CustomData { get; set; }

    /// <summary>
    /// Gets the untyped custom data of this layout.
    /// </summary>
    public object? UntypedCustomData => CustomData;

    #endregion

    #region Methods

    /// <summary>
    /// Creates a new <see cref="DeviceLayout"/> from the specified xml.
    /// </summary>
    /// <param name="stream">The stream that contains the layout to be loaded.</param>
    /// <returns>The deserialized <see cref="DeviceLayout{TCustomData, TCustomLedData}"/>.</returns>
    public static DeviceLayout<TCustomData, TCustomLedData>? Load(Stream stream)
    {
        try
        {
            XmlSerializer serializer = new(typeof(DeviceLayout<TCustomData, TCustomLedData>));
            DeviceLayout<TCustomData, TCustomLedData>? layout = serializer.Deserialize(stream) as DeviceLayout<TCustomData, TCustomLedData>;

            if (layout?.InternalLeds != null)
            {
                LedLayout<TCustomLedData>? lastLed = null;
                foreach (LedLayout<TCustomLedData> led in layout.InternalLeds)
                {
                    led.CalculateValues(layout, lastLed);
                    lastLed = led;
                }
            }

            return layout;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new <see cref="DeviceLayout"/> from the specified xml.
    /// </summary>
    /// <param name="path">The path to the xml file.</param>
    /// <returns>The deserialized <see cref="DeviceLayout"/>.</returns>
    public static DeviceLayout<TCustomData, TCustomLedData>? Load(string path)
    {
        if (!File.Exists(path)) return null;

        using Stream stream = File.OpenRead(path);
        return Load(stream);
    }

    #endregion
}
