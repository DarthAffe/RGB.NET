// ReSharper disable VirtualMemberNeverOverriden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic brush.
    /// </summary>
    public abstract class AbstractBrush : AbstractDecoratable<IBrushDecorator>, IBrush
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public BrushCalculationMode BrushCalculationMode { get; set; } = BrushCalculationMode.Relative;

        /// <inheritdoc />
        public double Brightness { get; set; }

        /// <inheritdoc />
        public double Opacity { get; set; }

        /// <inheritdoc />
        public IList<IColorCorrection> ColorCorrections { get; } = new List<IColorCorrection>();

        /// <inheritdoc />
        public Rectangle RenderedRectangle { get; protected set; }

        /// <inheritdoc />
        public Dictionary<BrushRenderTarget, Color> RenderedTargets { get; } = new Dictionary<BrushRenderTarget, Color>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractBrush"/> class.
        /// </summary>
        /// <param name="brightness">The overall percentage brightness of the brush. (default: 1.0)</param>
        /// <param name="opacity">The overall percentage opacity of the brush. (default: 1.0)</param>
        protected AbstractBrush(double brightness = 1, double opacity = 1)
        {
            this.Brightness = brightness;
            this.Opacity = opacity;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void PerformRender(Rectangle rectangle, IEnumerable<BrushRenderTarget> renderTargets)
        {
            RenderedRectangle = rectangle;
            RenderedTargets.Clear();

            foreach (BrushRenderTarget renderTarget in renderTargets)
            {
                Color color = new Color(GetColorAtPoint(rectangle, renderTarget)); // Clone the color, we don't want to have reference issues here and brushes might return the same color multiple times!}
                ApplyDecorators(rectangle, renderTarget, ref color);
                RenderedTargets[renderTarget] = color;
            }
        }

        /// <summary>
        /// Applies all attached and enabled decorators to the brush.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
        /// <param name="color">The <see cref="Color"/> to be modified.</param>
        protected virtual void ApplyDecorators(Rectangle rectangle, BrushRenderTarget renderTarget, ref Color color)
        {
            foreach (IBrushDecorator decorator in Decorators)
                if (decorator.IsEnabled)
                    decorator.ManipulateColor(rectangle, renderTarget, ref color);
        }

        /// <inheritdoc />
        public virtual void PerformFinalize()
        {
            List<BrushRenderTarget> renderTargets = RenderedTargets.Keys.ToList();
            foreach (BrushRenderTarget renderTarget in renderTargets)
                FinalizeColor(RenderedTargets[renderTarget]); // Cloning here again shouldn't be needed since we did this above.
        }

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        protected abstract Color GetColorAtPoint(Rectangle rectangle, BrushRenderTarget renderTarget);

        /// <summary>
        /// Finalizes the color by appliing the overall brightness and opacity.<br/>
        /// This method should always be the last call of a <see cref="GetColorAtPoint" /> implementation.
        /// If you overwrite this method please make sure that you never return the same color-object twice to prevent reference-issues!
        /// </summary>
        /// <param name="color">The color to finalize.</param>
        /// <returns>The finalized color.</returns>
        protected virtual void FinalizeColor(Color color)
        {
            foreach (IColorCorrection colorCorrection in ColorCorrections)
                colorCorrection.ApplyTo(color);

            // Since we use HSV to calculate there is no way to make a color 'brighter' than 100%
            // Be carefull with the naming: Since we use HSV the correct term is 'value' but outside we call it 'brightness'
            // THIS IS NOT A HSB CALCULATION!!!
            color.Value *= Brightness <= 0 ? 0 : (Brightness >= 1.0 ? 1.0 : Brightness);
            color.A = (byte)(color.A * (Opacity <= 0 ? 0 : (Opacity >= 1.0 ? 1.0 : Opacity)));
        }

        #endregion
    }
}
