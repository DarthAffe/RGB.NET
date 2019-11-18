namespace RGB.NET.Core
{
    public struct Scale
    {
        #region Properties & Fields

        public double Horizontal { get; }
        public double Vertical { get; }

        #endregion

        #region Constructors

        public Scale(double scale = 1.0) : this(scale, scale)
        { }

        public Scale(double horizontal, double vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        #endregion

        #region Methods

        public bool Equals(Scale other) => Horizontal.EqualsInTolerance(other.Horizontal) && Vertical.EqualsInTolerance(other.Vertical);
        public override bool Equals(object obj) => obj is Scale other && Equals(other);
        public override int GetHashCode() { unchecked { return (Horizontal.GetHashCode() * 397) ^ Vertical.GetHashCode(); } }

        public void Deconstruct(out double horizontalScale, out double verticalScale)
        {
            horizontalScale = Horizontal;
            verticalScale = Vertical;
        }

        #endregion

        #region Operators

        public static implicit operator Scale(double scale) => new Scale(scale);
        public static implicit operator Scale((double horizontal, double vertical) scale) => new Scale(scale.horizontal, scale.vertical);

        #endregion
    }
}
