using RGB.NET.Core;

namespace RGB.NET.Layout
{
    public interface ILedLayout
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="LedLayout"/>.
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Gets or sets the <see cref="RGB.NET.Core.Shape"/> of the <see cref="LedLayout"/>.
        /// </summary>
        Shape Shape { get; }

        /// <summary>
        /// Gets or sets the vecor-data representing a custom-shape of the <see cref="LedLayout"/>.
        /// </summary>
        string? ShapeData { get; }

        /// <summary>
        /// Gets the x-position of the <see cref="LedLayout"/>.
        /// </summary>
        double X { get; }

        /// <summary>
        /// Gets the y-position of the <see cref="LedLayout"/>.
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Gets the width of the <see cref="LedLayout"/>.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Gets the height of the <see cref="LedLayout"/>.
        /// </summary>
        double Height { get; }

        object? CustomData { get; }
    }
}
