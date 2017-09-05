// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Groups
{
    /// <summary>
    /// Represents a <see cref="RectangleLedGroup"/> containing <see cref="Led"/> which physically lay inside a <see cref="Core.Rectangle"/>.
    /// </summary>
    public class RectangleLedGroup : AbstractLedGroup
    {
        #region Properties & Fields

        private IList<Led> _ledCache;

        private Rectangle _rectangle;
        /// <summary>
        /// Gets or sets the <see cref="Core.Rectangle"/> the <see cref="Led"/>  should be taken from.
        /// </summary>
        public Rectangle Rectangle
        {
            get => _rectangle;
            set
            {
                Rectangle oldValue = _rectangle;
                if (SetProperty(ref _rectangle, value))
                {
                    if (oldValue != null)
                        oldValue.Changed -= RectangleChanged;

                    if (_rectangle != null)
                        _rectangle.Changed += RectangleChanged;

                    InvalidateCache();
                }
            }
        }

        private double _minOverlayPercentage;
        /// <summary>
        /// Gets or sets the minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Core.Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>.
        /// </summary>
        public double MinOverlayPercentage
        {
            get => _minOverlayPercentage;
            set
            {
                if (SetProperty(ref _minOverlayPercentage, value))
                    InvalidateCache();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="fromLed">They ID of the first <see cref="Led"/>  to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="toLed">They ID of the second <see cref="Led"/>  to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>. (default: 0.5)</param>
        /// <param name="autoAttach">(optional) Specifies whether this <see cref="RectangleLedGroup"/> should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(ILedId fromLed, ILedId toLed, double minOverlayPercentage = 0.5, bool autoAttach = true)
            : this(fromLed.Device[fromLed], toLed.Device[toLed], minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="fromLed">They first <see cref="Led"/>  to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="toLed">They second <see cref="Led"/>  to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>. (default: 0.5)</param>
        /// <param name="autoAttach">(optional) Specifies whether this <see cref="RectangleLedGroup"/> should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(Led fromLed, Led toLed, double minOverlayPercentage = 0.5, bool autoAttach = true)
            : this(new Rectangle(fromLed.LedRectangle, toLed.LedRectangle), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="fromPoint">They first point to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="toPoint">They second point to calculate the <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/> from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>. (default: 0.5)</param>
        /// <param name="autoAttach">(optional) Specifies whether this <see cref="RectangleLedGroup"/> should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(Point fromPoint, Point toPoint, double minOverlayPercentage = 0.5, bool autoAttach = true)
            : this(new Rectangle(fromPoint, toPoint), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="rectangle">The <see cref="Core.Rectangle"/> of this <see cref="RectangleLedGroup"/>.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>. (default: 0.5)</param>
        /// <param name="autoAttach">(optional) Specifies whether this <see cref="RectangleLedGroup"/> should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(Rectangle rectangle, double minOverlayPercentage = 0.5, bool autoAttach = true)
            : base(autoAttach)
        {
            this.Rectangle = rectangle;
            this.MinOverlayPercentage = minOverlayPercentage;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void OnAttach() => RGBSurface.Instance.SurfaceLayoutChanged += RGBSurfaceOnSurfaceLayoutChanged;

        /// <inheritdoc />
        public override void OnDetach() => RGBSurface.Instance.SurfaceLayoutChanged -= RGBSurfaceOnSurfaceLayoutChanged;

        private void RGBSurfaceOnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs args) => InvalidateCache();

        private void RectangleChanged(object sender, EventArgs eventArgs) => InvalidateCache();

        /// <summary>
        /// Gets a list containing all <see cref="Led"/> of this <see cref="RectangleLedGroup"/>.
        /// </summary>
        /// <returns>The list containing all <see cref="Led"/> of this <see cref="RectangleLedGroup"/>.</returns>
        public override IEnumerable<Led> GetLeds() => _ledCache ?? (_ledCache = RGBSurface.Instance.Leds.Where(x => x.LedRectangle.CalculateIntersectPercentage(Rectangle) >= MinOverlayPercentage).ToList());

        private void InvalidateCache() => _ledCache = null;

        #endregion
    }
}
