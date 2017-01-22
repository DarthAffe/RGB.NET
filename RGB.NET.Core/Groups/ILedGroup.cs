// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic ledgroup.
    /// </summary>
    public interface ILedGroup : IEffectTarget<ILedGroup>
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
    }
}
