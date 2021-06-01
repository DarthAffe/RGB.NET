// ReSharper disable EventNeverSubscribedTo.Global

using System;

namespace RGB.NET.Core
{
    public interface IPlaceable
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the location of the <see cref="IPlaceable"/>.
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Gets the size of the <see cref="IPlaceable"/>.
        /// </summary>
        Size Size { get; set; }

        /// <summary>
        /// Gets or sets the scale of the <see cref="IPlaceable"/>.
        /// </summary>
        Scale Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the <see cref="IPlaceable"/>.
        /// </summary>
        Rotation Rotation { get; set; }

        /// <summary>
        /// Gets  the actual location of the <see cref="IPlaceable"/>.
        /// This includes the <see cref="Rotation"/>.
        /// </summary>
        Point ActualLocation { get; }

        /// <summary>
        /// Gets the actual <see cref="Size"/> of the <see cref="IPlaceable"/>.
        /// This includes the <see cref="Scale"/>.
        /// </summary>
        Size ActualSize { get; }

        /// <summary>
        /// Gets a rectangle containing the whole <see cref="IPlaceable"/>.
        /// This includes <see cref="Location"/>, <see cref="Size"/>, <see cref="Scale"/> and <see cref="Rotation"/>.
        /// </summary>
        Rectangle Boundary { get; }

        #endregion

        #region Events

        event EventHandler<EventArgs> LocationChanged;
        event EventHandler<EventArgs> SizeChanged;
        event EventHandler<EventArgs> ScaleChanged;
        event EventHandler<EventArgs> RotationChanged;
        event EventHandler<EventArgs> ActualLocationChanged;
        event EventHandler<EventArgs> ActualSizeChanged;
        event EventHandler<EventArgs> BoundaryChanged;

        #endregion
    }
}
