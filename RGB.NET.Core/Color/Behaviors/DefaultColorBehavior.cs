namespace RGB.NET.Core
{
    public class DefaultColorBehavior : IColorBehavior
    {
        #region Properties & Fields

        private static DefaultColorBehavior _instance = new DefaultColorBehavior();
        /// <summary>
        /// Gets the singleton instance of <see cref="DefaultColorBehavior"/>.
        /// </summary>
        public static DefaultColorBehavior Instance { get; } = _instance;

        #endregion

        #region Constructors

        private DefaultColorBehavior()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the individual byte values of this <see cref="Color"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the individual byte values of this <see cref="Color"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
        public virtual string ToString(Color color) => $"[A: {color.GetA()}, R: {color.GetR()}, G: {color.GetG()}, B: {color.GetB()}]";

        /// <summary>
        /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
        public virtual bool Equals(Color color, object obj)
        {
            if (!(obj is Color)) return false;

            (double a, double r, double g, double b) = ((Color)obj).GetRGB();
            return color.A.EqualsInTolerance(a) && color.R.EqualsInTolerance(r) && color.G.EqualsInTolerance(g) && color.B.EqualsInTolerance(b);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Color" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
        public virtual int GetHashCode(Color color)
        {
            unchecked
            {
                int hashCode = color.A.GetHashCode();
                hashCode = (hashCode * 397) ^ color.R.GetHashCode();
                hashCode = (hashCode * 397) ^ color.G.GetHashCode();
                hashCode = (hashCode * 397) ^ color.B.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Blends a <see cref="Color"/> over this color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to blend.</param>
        public virtual Color Blend(Color baseColor, Color blendColor)
        {
            if (blendColor.A.EqualsInTolerance(0)) return baseColor;

            if (blendColor.A.EqualsInTolerance(1))
                return blendColor;

            double resultA = (1.0 - ((1.0 - blendColor.A) * (1.0 - baseColor.A)));
            double resultR = (((blendColor.R * blendColor.A) / resultA) + ((baseColor.R * baseColor.A * (1.0 - blendColor.A)) / resultA));
            double resultG = (((blendColor.G * blendColor.A) / resultA) + ((baseColor.G * baseColor.A * (1.0 - blendColor.A)) / resultA));
            double resultB = (((blendColor.B * blendColor.A) / resultA) + ((baseColor.B * baseColor.A * (1.0 - blendColor.A)) / resultA));

            return new Color(resultA, resultR, resultG, resultB);
        }

        #endregion
    }
}
