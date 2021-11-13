using System;

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for <see cref="Point"/> related things.
/// </summary>
public static class PointExtensions
{
    #region Methods

    /// <summary>
    /// Moves the specified <see cref="Point"/> by the specified amount.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to move.</param>
    /// <param name="x">The x-ammount to move.</param>
    /// <param name="y">The y-ammount to move.</param>
    /// <returns>The new location of the point.</returns>
    public static Point Translate(this in Point point, float x = 0, float y = 0) => new(point.X + x, point.Y + y);

    /// <summary>
    /// Rotates the specified <see cref="Point"/> by the specified amuont around the specified origin.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <param name="origin">The origin to rotate around. [0,0] if not set.</param>
    /// <returns>The new location of the point.</returns>
    public static Point Rotate(this in Point point, in Rotation rotation, in Point origin = new())
    {
        float sin = MathF.Sin(rotation.Radians);
        float cos = MathF.Cos(rotation.Radians);

        float x = point.X - origin.X;
        float y = point.Y - origin.Y;

        x = (x * cos) - (y * sin);
        y = (x * sin) + (y * cos);

        return new Point(x + origin.X, y + origin.Y);
    }

    #endregion
}