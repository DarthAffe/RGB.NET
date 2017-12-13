// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic ledgroup.
    /// </summary>
    public interface ILedGroup : IDecoratable<ILedGroupDecorator>
    {
        /// <summary>
        /// Gets or sets the <see cref="IBrush"/> which should be drawn over this <see cref="ILedGroup"/>.
        /// </summary>
        IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of this <see cref="ILedGroup"/> to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Gets a list containing all <see cref="Led"/> of this <see cref="ILedGroup"/>.
        /// </summary>
        /// <returns>The list containing all <see cref="Led"/> of this <see cref="ILedGroup"/>.</returns>
        IEnumerable<Led> GetLeds();

        /// <summary>
        /// Called when the <see cref="ILedGroup"/> is attached to the <see cref="RGBSurface"/>.
        /// </summary>
        void OnAttach();

        /// <summary>
        /// Called when the <see cref="ILedGroup"/> is detached from the <see cref="RGBSurface"/>.
        /// </summary>
        void OnDetach();
    }
}
