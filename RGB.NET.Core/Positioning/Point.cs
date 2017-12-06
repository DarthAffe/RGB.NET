// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a point consisting of a X- and a Y-position.
    /// </summary>
    [DebuggerDisplay("[X: {X}, Y: {Y}]")]
    public struct Point
    {
        #region Constants

        /// <summary>
        /// Gets a [NaN,NaN]-Point.
        /// </summary>
        public static Point Invalid => new Point(double.NaN, double.NaN);

        #endregion

        #region Properties & Fields

        /// <summary>
        /// Gets the X-position of this <see cref="Point"/>.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the Y-position of this <see cref="Point"/>.
        /// </summary>
        public double Y { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class using the provided values.
        /// </summary>
        /// <param name="x">The value used for the X-position.</param>
        /// <param name="y">The value used for the Y-position.</param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the <see cref="X"/>- and <see cref="Y"/>-position of this <see cref="Point"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the <see cref="X"/> and <see cref="Y"/>  of this <see cref="Point"/>. For example "[X: 100, Y: 20]".</returns>
        public override string ToString() => $"[X: {X}, Y: {Y}]";

        /// <summary>
        /// Tests whether the specified object is a <see cref="Point" /> and is equivalent to this <see cref="Point" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Point" /> equivalent to this <see cref="Point" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;

            Point comparePoint = (Point)obj;
            return ((double.IsNaN(X) && double.IsNaN(comparePoint.X)) || X.EqualsInTolerance(comparePoint.X))
                   && ((double.IsNaN(Y) && double.IsNaN(comparePoint.Y)) || Y.EqualsInTolerance(comparePoint.Y));
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Point" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Point" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Point" /> are equal.
        /// </summary>
        /// <param name="point1">The first <see cref="Point" /> to compare.</param>
        /// <param name="point2">The second <see cref="Point" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="point1" /> and <paramref name="point2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Point point1, Point point2) => point1.Equals(point2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Point" /> are equal.
        /// </summary>
        /// <param name="point1">The first <see cref="Point" /> to compare.</param>
        /// <param name="point2">The second <see cref="Point" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="point1" /> and <paramref name="point2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Point point1, Point point2) => !(point1 == point2);

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the addition of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the addition of the two provided <see cref="Point"/>.</returns>
        public static Point operator +(Point point1, Point point2) => new Point(point1.X + point2.X, point1.Y + point2.Y);

        /// <summary>
        /// Returns a new <see cref="Rectangle"/> created from the provided <see cref="Point"/> and <see cref="Size"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> of the rectangle.</param>
        /// <param name="size">The <see cref="Size"/> of the rectangle.</param>
        /// <returns>The rectangle created from the provided <see cref="Point"/> and <see cref="Size"/>.</returns>
        public static Rectangle operator +(Point point, Size size) => new Rectangle(point, size);

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the subtraction of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the subtraction of the two provided <see cref="Point"/>.</returns>
        public static Point operator -(Point point1, Point point2) => new Point(point1.X - point2.X, point1.Y - point2.Y);

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the multiplication of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the multiplication of the two provided <see cref="Point"/>.</returns>
        public static Point operator *(Point point1, Point point2) => new Point(point1.X * point2.X, point1.Y * point2.Y);

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the division of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the division of the two provided <see cref="Point"/>.</returns>
        public static Point operator /(Point point1, Point point2)
        {
            if (point2.X.EqualsInTolerance(0) || point2.Y.EqualsInTolerance(0)) return Invalid;
            return new Point(point1.X / point2.X, point1.Y / point2.Y);
        }

        #endregion
    }
}
