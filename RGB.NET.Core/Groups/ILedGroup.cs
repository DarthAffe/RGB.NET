// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic ledgroup.
/// </summary>
public interface ILedGroup : IDecoratable<ILedGroupDecorator>, IEnumerable<Led>
{
    /// <summary>
    /// Gets the surface this group is attached to or <c>null</c> if it is not attached to any surface.
    /// </summary>
    RGBSurface? Surface { get; internal set; }

    /// <summary>
    /// Gets a bool indicating if the group is attached to a surface.
    /// </summary>
    bool IsAttached => Surface != null;

    /// <summary>
    /// Gets or sets the <see cref="IBrush"/> which should be drawn over this <see cref="ILedGroup"/>.
    /// </summary>
    IBrush? Brush { get; set; }

    /// <summary>
    /// Gets or sets the z-index of this <see cref="ILedGroup"/> to allow ordering them before drawing. (lowest first) (default: 0)
    /// </summary>
    int ZIndex { get; set; }

    /// <summary>
    /// Called when the <see cref="ILedGroup"/> is attached to the <see cref="RGBSurface"/>.
    /// </summary>
    void OnAttach();

    /// <summary>
    /// Called when the <see cref="ILedGroup"/> is detached from the <see cref="RGBSurface"/>.
    /// </summary>
    void OnDetach();
}