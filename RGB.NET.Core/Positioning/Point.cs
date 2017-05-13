// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a point consisting of a X- and a Y-position.
    /// </summary>
    [DebuggerDisplay("[X: {X}, Y: {Y}]")]
    public class Point : AbstractBindable
    {
        #region Properties & Fields

        private double _x;
        /// <summary>
        /// Gets or sets the X-position of this <see cref="Point"/>.
        /// </summary>
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        private double _y;
        /// <summary>
        /// Gets or sets the Y-position of this <see cref="Point"/>.
        /// </summary>
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        public Point()
        { }

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
        public override string ToString()
        {
            return $"[X: {X}, Y: {Y}]";
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Point" /> and is equivalent to this <see cref="Point" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Point" /> equivalent to this <see cref="Point" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Point comparePoint = obj as Point;
            if (ReferenceEquals(comparePoint, null))
                return false;

            if (ReferenceEquals(this, comparePoint))
                return true;

            if (GetType() != comparePoint.GetType())
                return false;

            return X.EqualsInTolerance(comparePoint.X) && Y.EqualsInTolerance(comparePoint.Y);
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
        public static bool operator ==(Point point1, Point point2)
        {
            return ReferenceEquals(point1, null) ? ReferenceEquals(point2, null) : point1.Equals(point2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Point" /> are equal.
        /// </summary>
        /// <param name="point1">The first <see cref="Point" /> to compare.</param>
        /// <param name="point2">The second <see cref="Point" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="point1" /> and <paramref name="point2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the addition of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the addition of the two provided <see cref="Point"/>.</returns>
        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the substraction of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the substraction of the two provided <see cref="Point"/>.</returns>
        public static Point operator -(Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the multiplication of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the multiplication of the two provided <see cref="Point"/>.</returns>
        public static Point operator *(Point point1, Point point2)
        {
            return new Point(point1.X * point2.X, point1.Y * point2.Y);
        }

        /// <summary>
        /// Returns a new <see cref="Point"/> representing the division of the two provided <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>A new <see cref="Point"/> representing the division of the two provided <see cref="Point"/>.</returns>
        public static Point operator /(Point point1, Point point2)
        {
            if (point2.X.EqualsInTolerance(0) || point2.Y.EqualsInTolerance(0)) return new Point();
            return new Point(point1.X / point2.X, point1.Y / point2.Y);
        }

        #endregion
    }
}
