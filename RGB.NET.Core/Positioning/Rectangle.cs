// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RGB.NET.Core;

/// <summary>
/// Represents a rectangle defined by it's position and it's size.
/// </summary>
[DebuggerDisplay("[Location: {Location}, Size: {Size}]")]
public readonly struct Rectangle
{
    #region Properties & Fields

    /// <summary>
    /// Gets the <see cref="Point"/> representing the top-left corner of the <see cref="Rectangle"/>.
    /// </summary>
    public Point Location { get; }

    /// <summary>
    /// Gets the <see cref="Size"/> of the <see cref="Rectangle"/>.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets a new <see cref="Point"/> representing the center-point of the <see cref="Rectangle"/>.
    /// </summary>
    public Point Center { get; }

    /// <summary>
    /// Gets a bool indicating if both, the width and the height of the rectangle is greater than zero.
    /// <c>True</c> if the rectangle has a width or a height of zero; otherwise, <c>false</c>.
    /// </summary>
    public bool IsEmpty => (Size.Width <= FloatExtensions.TOLERANCE) || (Size.Height <= FloatExtensions.TOLERANCE);

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Rectangle" /> class using the provided values for <see cref="P:RGB.NET.Core.Rectangle.Location" /> ans <see cref="P:RGB.NET.Core.Rectangle.Size" />.
    /// </summary>
    /// <param name="x">The x-value of the <see cref="T:RGB.NET.Core.Location" /> of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    /// <param name="y">The y-value of the <see cref="T:RGB.NET.Core.Location" />-position of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    /// <param name="width">The width of the <see cref="T:RGB.NET.Core.Size"/> of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    /// <param name="height">The height of the <see cref="T:RGB.NET.Core.Size"/> of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    public Rectangle(float x, float y, float width, float height)
        : this(new Point(x, y), new Size(width, height))
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class using the <see cref="Location"/>(0,0) and the specified <see cref="Core.Size"/>.
    /// </summary>
    /// <param name="size">The size of of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    public Rectangle(Size size) : this(new Point(), size)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class using the specified <see cref="Point"/> and <see cref="Core.Size"/>.
    /// </summary>
    /// <param name="location">The location of this of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    /// <param name="size">The size of of this <see cref="T:RGB.NET.Core.Rectangle" />.</param>
    public Rectangle(Point location, Size size)
    {
        this.Location = location;
        this.Size = size;

        Center = new Point(Location.X + (Size.Width / 2.0f), Location.Y + (Size.Height / 2.0f));
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Rectangle" /> class using the specified array of <see cref="T:RGB.NET.Core.Rectangle" />.
    /// The <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" /> is calculated to completely contain all rectangles provided as parameters.
    /// </summary>
    /// <param name="rectangles">The array of <see cref="T:RGB.NET.Core.Rectangle" /> used to calculate the <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" />.</param>
    public Rectangle(params Rectangle[] rectangles)
        : this(rectangles.AsEnumerable())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class using the specified list of <see cref="Rectangle"/>.
    /// The <see cref="Location"/> and <see cref="Size"/> is calculated to completely contain all rectangles provided as parameters.
    /// </summary>
    /// <param name="rectangles">The list of <see cref="Rectangle"/> used to calculate the <see cref="Location"/> and <see cref="Size"/>.</param>
    public Rectangle(IEnumerable<Rectangle> rectangles)
    {
        bool hasPoint = false;
        float posX = float.MaxValue;
        float posY = float.MaxValue;
        float posX2 = float.MinValue;
        float posY2 = float.MinValue;

        foreach (Rectangle rectangle in rectangles)
        {
            hasPoint = true;
            posX = Math.Min(posX, rectangle.Location.X);
            posY = Math.Min(posY, rectangle.Location.Y);
            posX2 = Math.Max(posX2, rectangle.Location.X + rectangle.Size.Width);
            posY2 = Math.Max(posY2, rectangle.Location.Y + rectangle.Size.Height);
        }

        (Point location, Size size) = hasPoint ? InitializeFromPoints(new Point(posX, posY), new Point(posX2, posY2)) : InitializeFromPoints(new Point(0, 0), new Point(0, 0));
        Location = location;
        Size = size;
        Center = new Point(Location.X + (Size.Width / 2.0f), Location.Y + (Size.Height / 2.0f));
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Rectangle" /> class using the specified array of <see cref="T:RGB.NET.Core.Point" />.
    /// The <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" /> is calculated to contain all points provided as parameters.
    /// </summary>
    /// <param name="points">The array of <see cref="T:RGB.NET.Core.Point" /> used to calculate the <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" />.</param>
    public Rectangle(params Point[] points)
        : this(points.AsEnumerable())
    { }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Rectangle" /> class using the specified list of <see cref="T:RGB.NET.Core.Point" />.
    /// The <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" /> is calculated to contain all points provided as parameters.
    /// </summary>
    /// <param name="points">The list of <see cref="T:RGB.NET.Core.Point" /> used to calculate the <see cref="P:RGB.NET.Core.Rectangle.Location" /> and <see cref="P:RGB.NET.Core.Rectangle.Size" />.</param>
    public Rectangle(IEnumerable<Point> points)
        : this()
    {
        bool hasPoint = false;
        float posX = float.MaxValue;
        float posY = float.MaxValue;
        float posX2 = float.MinValue;
        float posY2 = float.MinValue;

        foreach (Point point in points)
        {
            hasPoint = true;
            posX = Math.Min(posX, point.X);
            posY = Math.Min(posY, point.Y);
            posX2 = Math.Max(posX2, point.X);
            posY2 = Math.Max(posY2, point.Y);
        }

        (Point location, Size size) = hasPoint ? InitializeFromPoints(new Point(posX, posY), new Point(posX2, posY2)) : InitializeFromPoints(new Point(0, 0), new Point(0, 0));

        Location = location;
        Size = size;
        Center = new Point(Location.X + (Size.Width / 2.0f), Location.Y + (Size.Height / 2.0f));
    }

    #endregion

    #region Methods

    private static (Point location, Size size) InitializeFromPoints(in Point point1, in Point point2)
    {
        float posX = Math.Min(point1.X, point2.X);
        float posY = Math.Min(point1.Y, point2.Y);
        float width = Math.Max(point1.X, point2.X) - posX;
        float height = Math.Max(point1.Y, point2.Y) - posY;

        return (new Point(posX, posY), new Size(width, height));
    }

    /// <summary>
    /// Converts the <see cref="Location"/>- and <see cref="Size"/>-position of this <see cref="Rectangle"/> to a human-readable string.
    /// </summary>
    /// <returns>A string that contains the <see cref="Location"/> and <see cref="Size"/>  of this <see cref="Rectangle"/>. For example "[Location: [X: 100, Y: 10], Size: [Width: 20, Height: [40]]".</returns>
    public override string ToString() => $"[Location: {Location}, Size: {Size}]";

    /// <summary>
    /// Tests whether the specified object is a <see cref="Rectangle" /> and is equivalent to this <see cref="Rectangle" />.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Rectangle" /> equivalent to this <see cref="Rectangle" />; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Rectangle compareRect)
            return false;

        if (GetType() != compareRect.GetType())
            return false;

        return (Location == compareRect.Location) && (Size == compareRect.Size);
    }

    /// <summary>
    /// Returns a hash code for this <see cref="Rectangle" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Rectangle" />.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Location.GetHashCode();
            hashCode = (hashCode * 397) ^ Size.GetHashCode();
            return hashCode;
        }
    }

    #endregion

    #region Operators

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Rectangle" /> are equal.
    /// </summary>
    /// <param name="rectangle1">The first <see cref="Rectangle" /> to compare.</param>
    /// <param name="rectangle2">The second <see cref="Rectangle" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="rectangle1" /> and <paramref name="rectangle2" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(in Rectangle rectangle1, in Rectangle rectangle2) => rectangle1.Equals(rectangle2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Rectangle" /> are equal.
    /// </summary>
    /// <param name="rectangle1">The first <see cref="Rectangle" /> to compare.</param>
    /// <param name="rectangle2">The second <see cref="Rectangle" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="rectangle1" /> and <paramref name="rectangle2" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(in Rectangle rectangle1, in Rectangle rectangle2) => !(rectangle1 == rectangle2);

    // DarthAffe 20.02.2021: Used for normalization
    /// <summary>
    /// Returns a <see cref="Rectangle"/> normalized to the specified reference.
    /// </summary>
    /// <param name="rectangle1">The rectangle to nromalize.</param>
    /// <param name="rectangle2">The reference used for normalization.</param>
    /// <returns>A normalized rectangle.</returns>
    public static Rectangle operator /(in Rectangle rectangle1, in Rectangle rectangle2)
    {
        float x = rectangle1.Location.X / (rectangle2.Size.Width - rectangle2.Location.X);
        float y = rectangle1.Location.Y / (rectangle2.Size.Height - rectangle2.Location.Y);
        Size size = rectangle1.Size / rectangle2.Size;
        return new Rectangle(new Point(x, y), size);
    }

    #endregion
}