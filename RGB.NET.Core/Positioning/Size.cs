// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a size consisting of a width and a height.
    /// </summary>
    [DebuggerDisplay("[Width: {Width}, Height: {Height}]")]
    public class Size : AbstractBindable
    {
        #region Properties & Fields

        private double _width;
        /// <summary>
        /// Gets or sets the width component value of this <see cref="Size"/>.
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        private double _height;
        /// <summary>
        /// Gets or sets the height component value of this <see cref="Size"/>.
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        public Size()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> using the provided size to define a square.
        /// </summary>
        /// <param name="size">The size used for the <see cref="Width"/> component value and the <see cref="Height"/> component value.</param>
        public Size(double size)
            : this(size, size)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class using the provided values.
        /// </summary>
        /// <param name="width">The size used for the <see cref="Width"/> component value.</param>
        /// <param name="height">The size used for the <see cref="Height"/> component value.</param>
        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the <see cref="Width"/> and <see cref="Height"/> of this <see cref="Size"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the <see cref="Width"/> and <see cref="Height"/>  of this <see cref="Size"/>. For example "[Width: 100, Height: 20]".</returns>
        public override string ToString()
        {
            return $"[Width: {Width}, Height: {Height}]";
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Size" /> and is equivalent to this <see cref="Size" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Size" /> equivalent to this <see cref="Size" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Size compareSize = obj as Size;
            if (ReferenceEquals(compareSize, null))
                return false;

            if (ReferenceEquals(this, compareSize))
                return true;

            if (GetType() != compareSize.GetType())
                return false;

            return Width.EqualsInTolerance(compareSize.Width) && Height.EqualsInTolerance(compareSize.Height);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Size" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Size" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Size" /> are equal.
        /// </summary>
        /// <param name="size1">The first <see cref="Size" /> to compare.</param>
        /// <param name="size2">The second <see cref="Size" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="size1" /> and <paramref name="size2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Size size1, Size size2)
        {
            return ReferenceEquals(size1, null) ? ReferenceEquals(size2, null) : size1.Equals(size2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Size" /> are equal.
        /// </summary>
        /// <param name="size1">The first <see cref="Size" /> to compare.</param>
        /// <param name="size2">The second <see cref="Size" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="size1" /> and <paramref name="size2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        /// <summary>
        /// Returns a new <see cref="Size"/> representing the addition of the two provided <see cref="Size"/>.
        /// </summary>
        /// <param name="size1">The first <see cref="Size"/>.</param>
        /// <param name="size2">The second <see cref="Size"/>.</param>
        /// <returns>A new <see cref="Size"/> representing the addition of the two provided <see cref="Size"/>.</returns>
        public static Size operator +(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        /// <summary>
        /// Returns a new <see cref="Size"/> representing the substraction of the two provided <see cref="Size"/>.
        /// </summary>
        /// <param name="size1">The first <see cref="Size"/>.</param>
        /// <param name="size2">The second <see cref="Size"/>.</param>
        /// <returns>A new <see cref="Size"/> representing the substraction of the two provided <see cref="Size"/>.</returns>
        public static Size operator -(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }

        /// <summary>
        /// Returns a new <see cref="Size"/> representing the multiplication of the two provided <see cref="Size"/>.
        /// </summary>
        /// <param name="size1">The first <see cref="Size"/>.</param>
        /// <param name="size2">The second <see cref="Size"/>.</param>
        /// <returns>A new <see cref="Size"/> representing the multiplication of the two provided <see cref="Size"/>.</returns>
        public static Size operator *(Size size1, Size size2)
        {
            return new Size(size1.Width * size2.Width, size1.Height * size2.Height);
        }

        /// <summary>
        /// Returns a new <see cref="Size"/> representing the division of the two provided <see cref="Size"/>.
        /// </summary>
        /// <param name="size1">The first <see cref="Size"/>.</param>
        /// <param name="size2">The second <see cref="Size"/>.</param>
        /// <returns>A new <see cref="Size"/> representing the division of the two provided <see cref="Size"/>.</returns>
        public static Size operator /(Size size1, Size size2)
        {
            if (size2.Width.EqualsInTolerance(0) || size2.Height.EqualsInTolerance(0)) return new Size();
            return new Size(size1.Width / size2.Width, size1.Height / size2.Height);
        }

        #endregion
    }
}
