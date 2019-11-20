using System;

namespace RGB.NET.Core
{
    public struct Rotation
    {
        #region Constants

        private const double TWO_PI = Math.PI * 2.0;
        private const double RADIANS_DEGREES_CONVERSION = 180.0 / Math.PI;
        private const double DEGREES_RADIANS_CONVERSION = Math.PI / 180.0;

        #endregion

        #region Properties & Fields

        public double Degrees { get; }
        public double Radians { get; }

        #endregion

        #region Constructors

        public Rotation(double degrees)
            : this(degrees, degrees * DEGREES_RADIANS_CONVERSION)
        { }

        private Rotation(double degrees, double radians)
        {
            this.Degrees = degrees % 360.0;
            this.Radians = radians % TWO_PI;
        }

        #endregion

        #region Methods

        public static Rotation FromDegrees(double degrees) => new Rotation(degrees);
        public static Rotation FromRadians(double radians) => new Rotation(radians * RADIANS_DEGREES_CONVERSION, radians);

        public bool Equals(Rotation other) => Degrees.EqualsInTolerance(other.Degrees);
        public override bool Equals(object obj) => obj is Rotation other && Equals(other);
        public override int GetHashCode() => Degrees.GetHashCode();

        #endregion

        #region Operators

        public static implicit operator Rotation(double rotation) => new Rotation(rotation);
        public static implicit operator double(Rotation rotation) => rotation.Degrees;

        #endregion
    }
}
