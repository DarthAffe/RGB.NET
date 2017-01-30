using System;
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
        [XmlElement("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LayoutShape"/> of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlElement("Shape")]
        public LayoutShape Shape { get; set; }

        /// <summary>
        /// Gets or sets the x-position of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlElement("X")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-position of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlElement("Y")]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlElement("Width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="LedLayout"/>.
        /// </summary>
        [XmlElement("Height")]
        public double Height { get; set; }

        #endregion
    }
}
