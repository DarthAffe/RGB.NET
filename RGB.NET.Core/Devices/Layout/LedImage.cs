using System;
using System.Xml.Serialization;

namespace RGB.NET.Core.Layout
{
    /// <summary>
    /// Represents the serializable image-data of a specific <see cref="Led"/>.
    /// </summary>
    [Serializable]
    [XmlRoot("LedImage")]
    public class LedImage
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="LedImage"/>.
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the image of the <see cref="LedImage"/>.
        /// </summary>
        [XmlAttribute("Image")]
        public string Image { get; set; }
    }
}
