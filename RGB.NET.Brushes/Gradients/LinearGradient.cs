// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Brushes.Gradients
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a linear interpolated gradient with n stops.
    /// </summary>
    public class LinearGradient : AbstractGradient
    {
        #region Properties & Fields

        private bool _isOrderedGradientListDirty = true;
        private LinkedList<GradientStop> _orderedGradientStops;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.LinearGradient" /> class.
        /// </summary>
        public LinearGradient()
        {
            Initialize();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.LinearGradient" /> class.
        /// </summary>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(params GradientStop[] gradientStops)
            : base(gradientStops)
        {
            Initialize();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.AbstractGradient" /> class.
        /// </summary>
        /// <param name="wrapGradient">Specifies whether the gradient should wrapp or not (see <see cref="P:RGB.NET.Brushes.Gradients.AbstractGradient.WrapGradient" /> for an example of what this means).</param>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(bool wrapGradient, params GradientStop[] gradientStops)
            : base(wrapGradient, gradientStops)
        {
            Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            void OnGradientStopOnPropertyChanged(object sender, PropertyChangedEventArgs args) => _isOrderedGradientListDirty = true;

            foreach (GradientStop gradientStop in GradientStops)
                gradientStop.PropertyChanged += OnGradientStopOnPropertyChanged;

            GradientStops.CollectionChanged += (sender, args) =>
                                               {
                                                   if (args.OldItems != null)
                                                       foreach (GradientStop gradientStop in args.OldItems)
                                                           gradientStop.PropertyChanged -= OnGradientStopOnPropertyChanged;

                                                   if (args.NewItems != null)
                                                       foreach (GradientStop gradientStop in args.NewItems)
                                                           gradientStop.PropertyChanged += OnGradientStopOnPropertyChanged;
                                               };
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the linear interpolated <see cref="T:RGB.NET.Core.Color" /> at the given offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the color from.</param>
        /// <returns>The <see cref="T:RGB.NET.Core.Color" /> at the specific offset.</returns>
        public override Color GetColor(double offset)
        {
            if (GradientStops.Count == 0) return Color.Transparent;
            if (GradientStops.Count == 1) return GradientStops[0].Color;

            if (_isOrderedGradientListDirty)
                _orderedGradientStops = new LinkedList<GradientStop>(GradientStops.OrderBy(x => x.Offset));

            (GradientStop gsBefore, GradientStop gsAfter) = GetEnclosingGradientStops(offset, _orderedGradientStops, WrapGradient);

            double blendFactor = 0;
            if (!gsBefore.Offset.Equals(gsAfter.Offset))
                blendFactor = ((offset - gsBefore.Offset) / (gsAfter.Offset - gsBefore.Offset));

            byte colA = (byte)(((gsAfter.Color.A - gsBefore.Color.A) * blendFactor) + gsBefore.Color.A);
            byte colR = (byte)(((gsAfter.Color.R - gsBefore.Color.R) * blendFactor) + gsBefore.Color.R);
            byte colG = (byte)(((gsAfter.Color.G - gsBefore.Color.G) * blendFactor) + gsBefore.Color.G);
            byte colB = (byte)(((gsAfter.Color.B - gsBefore.Color.B) * blendFactor) + gsBefore.Color.B);

            return new Color(colA, colR, colG, colB);
        }

        /// <summary>
        /// Get the two <see cref="GradientStop"/>s encapsulating the given offset.
        /// </summary>
        /// <param name="offset">The reference offset.</param>
        /// <param name="orderedStops">The ordered list of <see cref="GradientStop"/> to choose from.</param>
        /// <param name="wrap">Bool indicating if the gradient should be wrapped or not.</param>
        /// <returns></returns>
        protected virtual (GradientStop gsBefore, GradientStop gsAfter) GetEnclosingGradientStops(double offset, LinkedList<GradientStop> orderedStops, bool wrap)
        {
            LinkedList<GradientStop> gradientStops = new LinkedList<GradientStop>(orderedStops);

            if (wrap)
            {
                GradientStop gsBefore, gsAfter;

                do
                {
                    gsBefore = gradientStops.LastOrDefault(n => n.Offset <= offset);
                    if (gsBefore == null)
                    {
                        GradientStop lastStop = gradientStops.Last.Value;
                        gradientStops.AddFirst(new GradientStop(lastStop.Offset - 1, lastStop.Color));
                        gradientStops.RemoveLast();
                    }

                    gsAfter = gradientStops.FirstOrDefault(n => n.Offset >= offset);
                    if (gsAfter == null)
                    {
                        GradientStop firstStop = gradientStops.First.Value;
                        gradientStops.AddLast(new GradientStop(firstStop.Offset + 1, firstStop.Color));
                        gradientStops.RemoveFirst();
                    }

                } while ((gsBefore == null) || (gsAfter == null));

                return (gsBefore, gsAfter);
            }

            offset = ClipOffset(offset);
            return (gradientStops.Last(n => n.Offset <= offset), gradientStops.First(n => n.Offset >= offset));
        }

        #endregion
    }
}
