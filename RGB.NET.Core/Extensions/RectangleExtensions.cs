using System;

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for the work with rectangles.
/// </summary>
public static class RectangleExtensions
{
    #region Methods

    /// <summary>
    /// Sets the <see cref="Rectangle.Location"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="location">The new location of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetLocation(this in Rectangle rect, in Point location) => new(location, rect.Size);

    /// <summary>
    /// Sets the <see cref="Point.X"/> of the <see cref="Rectangle.Location"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="x">The new x-location of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetX(this in Rectangle rect, float x) => new(new Point(x, rect.Location.Y), rect.Size);

    /// <summary>
    /// Sets the <see cref="Point.Y"/> of the <see cref="Rectangle.Location"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="y">The new y-location of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetY(this in Rectangle rect, float y) => new(new Point(rect.Location.X, y), rect.Size);

    /// <summary>
    /// Sets the <see cref="Rectangle.Size"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="size">The new size of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetSize(this in Rectangle rect, in Size size) => new(rect.Location, size);

    /// <summary>
    /// Sets the <see cref="Size.Width"/> of the <see cref="Rectangle.Size"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="width">The new width of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetWidth(this in Rectangle rect, float width) => new(rect.Location, new Size(width, rect.Size.Height));

    /// <summary>
    /// Sets the <see cref="Size.Height"/> of the <see cref="Rectangle.Size"/> of the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to modify.</param>
    /// <param name="height">The new height of the rectangle.</param>
    /// <returns>The modified <see cref="Rectangle"/>.</returns>
    public static Rectangle SetHeight(this in Rectangle rect, float height) => new(rect.Location, new Size(rect.Size.Width, height));

    /// <summary>
    /// Calculates the percentage of intersection of a rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to calculate the intersection for.</param>
    /// <param name="intersectingRect">The intersecting rectangle.</param>
    /// <returns>The percentage of intersection.</returns>
    public static float CalculateIntersectPercentage(this in Rectangle rect, in Rectangle intersectingRect)
    {
        if (rect.IsEmpty || intersectingRect.IsEmpty) return 0;

        Rectangle intersection = rect.CalculateIntersection(intersectingRect);
        return (intersection.Size.Width * intersection.Size.Height) / (rect.Size.Width * rect.Size.Height);
    }

    /// <summary>
    /// Calculates the <see cref="Rectangle"/> representing the intersection of this <see cref="Rectangle"/> and the one provided as parameter.
    /// </summary>
    /// <param name="rect">The rectangle to calculate the intersection for.</param>
    /// <param name="intersectingRectangle">The intersecting <see cref="Rectangle"/>.</param>
    /// <returns>A new <see cref="Rectangle"/> representing the intersection this <see cref="Rectangle"/> and the one provided as parameter.</returns>
    public static Rectangle CalculateIntersection(this in Rectangle rect, in Rectangle intersectingRectangle)
    {
        float x1 = Math.Max(rect.Location.X, intersectingRectangle.Location.X);
        float x2 = Math.Min(rect.Location.X + rect.Size.Width, intersectingRectangle.Location.X + intersectingRectangle.Size.Width);

        float y1 = Math.Max(rect.Location.Y, intersectingRectangle.Location.Y);
        float y2 = Math.Min(rect.Location.Y + rect.Size.Height, intersectingRectangle.Location.Y + intersectingRectangle.Size.Height);

        if ((x2 >= x1) && (y2 >= y1))
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);

        return new Rectangle();
    }

    /// <summary>
    /// Determines if the specified <see cref="Point"/> is contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect">The containing rectangle.</param>
    /// <param name="point">The <see cref="Point"/> to test.</param>
    /// <returns><c>true</c> if the rectangle contains the specified point; otherwise <c>false</c>.</returns>
    public static bool Contains(this in Rectangle rect, in Point point) => rect.Contains(point.X, point.Y);

    /// <summary>
    /// Determines if the specified location is contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect">The containing rectangle.</param>
    /// <param name="x">The X-location to test.</param>
    /// <param name="y">The Y-location to test.</param>
    /// <returns><c>true</c> if the rectangle contains the specified coordinates; otherwise <c>false</c>.</returns>
    public static bool Contains(this in Rectangle rect, float x, float y) => (rect.Location.X <= x) && (x < (rect.Location.X + rect.Size.Width))
                                                                          && (rect.Location.Y <= y) && (y < (rect.Location.Y + rect.Size.Height));

    /// <summary>
    /// Determines if the specified <see cref="Rectangle"/> is contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect">The containing rectangle.</param>
    /// <param name="rect2">The <see cref="Rectangle"/> to test.</param>
    /// <returns><c>true</c> if the rectangle contains the specified rect; otherwise <c>false</c>.</returns>
    public static bool Contains(this in Rectangle rect, in Rectangle rect2) => (rect.Location.X <= rect2.Location.X) && ((rect2.Location.X + rect2.Size.Width) <= (rect.Location.X + rect.Size.Width))
                                                                            && (rect.Location.Y <= rect2.Location.Y) && ((rect2.Location.Y + rect2.Size.Height) <= (rect.Location.Y + rect.Size.Height));

    /// <summary>
    /// Moves the specified <see cref="Rectangle"/> by the specified amount.
    /// </summary>
    /// <param name="rect">The <see cref="Rectangle"/> to move.</param>
    /// <param name="point">The amount to move.</param>
    /// <returns>The moved rectangle.</returns>
    public static Rectangle Translate(this in Rectangle rect, in Point point) => rect.Translate(point.X, point.Y);

    /// <summary>
    /// Moves the specified <see cref="Rectangle"/> by the specified amount.
    /// </summary>
    /// <param name="rect">The <see cref="Rectangle"/> to move.</param>
    /// <param name="x">The x-ammount to move.</param>
    /// <param name="y">The y-ammount to move.</param>
    /// <returns>The moved rectangle.</returns>
    public static Rectangle Translate(this in Rectangle rect, float x = 0, float y = 0) => new(rect.Location.Translate(x, y), rect.Size);

    /// <summary>
    /// Rotates the specified <see cref="Rectangle"/> by the specified amuont around the specified origin.
    /// </summary>
    /// <remarks>
    /// The returned array of <see cref="Point"/> is filled with the new locations of the rectangle clockwise starting from the top left:
    /// [0] = top left
    /// [1] = top right
    /// [2] = bottom right
    /// [3] = bottom left
    /// </remarks>
    /// <param name="rect">The <see cref="Rectangle"/> to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <param name="origin">The origin to rotate around. [0,0] if not set.</param>
    /// <returns>A array of <see cref="Point"/> containing the new locations of the corners of the original rectangle.</returns>
    public static Point[] Rotate(this in Rectangle rect, in Rotation rotation, in Point origin = new())
    {
        Point[] points = {
                             rect.Location, // top left
                             new(rect.Location.X + rect.Size.Width, rect.Location.Y), // top right
                             new(rect.Location.X + rect.Size.Width, rect.Location.Y + rect.Size.Height), // bottom right
                             new(rect.Location.X, rect.Location.Y + rect.Size.Height), // bottom right
                         };

        float sin = MathF.Sin(rotation.Radians);
        float cos = MathF.Cos(rotation.Radians);

        for (int i = 0; i < points.Length; i++)
        {
            Point point = points[i];
            point = new Point(point.X - origin.X, point.Y - origin.Y);
            point = new Point((point.X * cos) - (point.Y * sin), (point.X * sin) + (point.Y * cos));
            points[i] = new Point(point.X + origin.X, point.Y + origin.Y);
        }

        return points;
    }

    #endregion
}