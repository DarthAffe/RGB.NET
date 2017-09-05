// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global

using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic brush.
    /// </summary>
    public interface IBrush : IDecoratable<IBrushDecorator>
    {
        /// <summary>
        /// Gets or sets if the <see cref="IBrush"/> is enabled and will be drawn on an update.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the calculation mode used for the rectangle/points used for color-selection in brushes.
        /// </summary>
        BrushCalculationMode BrushCalculationMode { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage brightness of the <see cref="IBrush"/>.
        /// </summary>
        double Brightness { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage opacity of the <see cref="IBrush"/>.
        /// </summary>
        double Opacity { get; set; }

        /// <summary>
        /// Gets a list of <see cref="IColorCorrection"/> used to correct the colors of the <see cref="IBrush"/>.
        /// </summary>
        IList<IColorCorrection> ColorCorrections { get; }

        /// <summary>
        /// Gets the <see cref="RenderedRectangle"/> used in the last render pass.
        /// </summary>
        Rectangle RenderedRectangle { get; }

        /// <summary>
        /// Gets a dictionary containing all <see cref="Color"/> for <see cref="BrushRenderTarget"/> calculated in the last render pass.
        /// </summary>
        Dictionary<BrushRenderTarget, Color> RenderedTargets { get; }

        /// <summary>
        /// Performs the render pass of the <see cref="IBrush"/> and calculates the raw <see cref="Color"/> for all requested <see cref="BrushRenderTarget"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> in which the brush should be drawn.</param>
        /// <param name="renderTargets">The <see cref="BrushRenderTarget"/> (keys/points) of which the color should be calculated.</param>
        void PerformRender(Rectangle rectangle, IEnumerable<BrushRenderTarget> renderTargets);

        /// <summary>
        /// Performs the finalize pass of the <see cref="IBrush"/> and calculates the final <see cref="ColorCorrections"/> for all previously calculated <see cref="BrushRenderTarget"/>.
        /// </summary>
        void PerformFinalize();
    }
}