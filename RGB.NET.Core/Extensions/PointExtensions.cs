using System;

namespace RGB.NET.Core
{
    public static class PointExtensions
    {
        #region Methods

        /// <summary>
        /// Moves the specified <see cref="Point"/> by the given amount.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> to move.</param>
        /// <param name="x">The x-ammount to move.</param>
        /// <param name="y">The y-ammount to move.</param>
        /// <returns>The new location of the point.</returns>
        public static Point Translate(this Point point, double x = 0, double y = 0) => new Point(point.X + x, point.Y + y);

        /// <summary>
        /// Rotates the specified <see cref="Point"/> by the given amuont around the given origin.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> to rotate.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin to rotate around. [0,0] if not set.</param>
        /// <returns>The new location of the point.</returns>
        public static Point Rotate(this Point point, Rotation rotation, Point origin = new Point())
        {
            double sin = Math.Sin(rotation.Radians);
            double cos = Math.Cos(rotation.Radians);

            point = new Point(point.X - origin.X, point.Y - origin.Y);
            point = new Point((point.X * cos) - (point.Y * sin), (point.X * sin) + (point.Y * cos));
            return new Point(point.X + origin.X, point.Y + origin.Y); ;
        }

        #endregion
    }
}
