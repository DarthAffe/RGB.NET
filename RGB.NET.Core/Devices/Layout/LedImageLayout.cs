using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RGB.NET.Core.Layout
{
    /// <summary>
    /// Represents the serializable collection of <see cref="LedImage"/> for a specific layout.
    /// </summary>
    [Serializable]
    [XmlRoot("LedImageLayout")]
    public class LedImageLayout
    {
        /// <summary>
        /// Gets or sets the layout of the <see cref="LedImage"/>.
        /// </summary>
        [XmlAttribute("Layout")]
        [DefaultValue(null)]
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="LedImage"/> representing the images of all the <see cref="Led"/> of the represented layout.
        /// </summary>
        [XmlArray("LedImages")]
        public List<LedImage> LedImages { get; set; } = new List<LedImage>();
    }
}
