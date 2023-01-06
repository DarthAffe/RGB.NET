using System.Collections;
using System.Collections.Generic;

namespace RGB.NET.Core;

/// <inheritdoc cref="AbstractDecoratable{T}" />
/// <inheritdoc cref="ILedGroup" />
/// <summary>
/// Represents a generic <see cref="T:RGB.NET.Core.AbstractLedGroup" />.
/// </summary>
public abstract class AbstractLedGroup : AbstractDecoratable<ILedGroupDecorator>, ILedGroup
{
    #region Properties & Fields

    RGBSurface? ILedGroup.Surface { get; set; }

    /// <inheritdoc cref="ILedGroup.Surface" />
    public RGBSurface? Surface => ((ILedGroup)this).Surface;

    /// <inheritdoc />
    public IBrush? Brush { get; set; }

    /// <inheritdoc />
    public int ZIndex { get; set; } = 0;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractLedGroup"/> class.
    /// </summary>
    protected AbstractLedGroup(RGBSurface? attachTo)
    {
        attachTo?.Attach(this);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a enumerable containing all leds in this group.
    /// </summary>
    /// <returns>A enumerable containing all leds of this group.</returns>
    protected abstract IEnumerable<Led> GetLeds();

    /// <inheritdoc />
    public virtual void OnAttach() { }

    /// <inheritdoc />
    public virtual void OnDetach() { }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<Led> GetEnumerator() => GetLeds().GetEnumerator();

    #endregion
}