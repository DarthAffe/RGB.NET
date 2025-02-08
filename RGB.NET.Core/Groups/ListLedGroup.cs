// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Threading;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a ledgroup containing arbitrary <see cref="T:RGB.NET.Core.Led" />.
/// </summary>
public sealed class ListLedGroup : AbstractLedGroup
{
    #region Properties & Fields

    private readonly ActionDisposable _unlockDisposable;

    /// <summary>
    /// Gets the list containing the <see cref="Led"/> of this <see cref="ListLedGroup"/>.
    /// </summary>
    private readonly IList<Led> _groupLeds = [];

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    public ListLedGroup(RGBSurface? surface)
        : base(surface)
    {
        _unlockDisposable = new ActionDisposable(Unlock);
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    /// <param name="leds">The initial <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Groups.ListLedGroup" />.</param>
    public ListLedGroup(RGBSurface? surface, IEnumerable<Led> leds)
        : base(surface)
    {
        _unlockDisposable = new ActionDisposable(Unlock);

        AddLeds(leds);
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    /// <param name="leds">The initial <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Groups.ListLedGroup" />.</param>
    public ListLedGroup(RGBSurface? surface, params Led[] leds)
        : base(surface)
    {
        _unlockDisposable = new ActionDisposable(Unlock);

        AddLeds(leds);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified LED(s) to this <see cref="ListLedGroup"/>.
    /// </summary>
    /// <param name="leds">The LED(s) to add.</param>
    public void AddLed(params Led[] leds) => AddLeds(leds);

    /// <summary>
    /// Adds the specified <see cref="Led"/> to this <see cref="ListLedGroup"/>.
    /// </summary>
    /// <param name="leds">The <see cref="Led"/> to add.</param>
    public void AddLeds(IEnumerable<Led> leds)
    {
        lock (_groupLeds)
            foreach (Led led in leds)
                if (!ContainsLed(led))
                    _groupLeds.Add(led);
    }

    /// <summary>
    /// Removes the specified LED(s) from this <see cref="ListLedGroup"/>.
    /// </summary>
    /// <param name="leds">The LED(s) to remove.</param>
    public void RemoveLed(params Led[] leds) => RemoveLeds(leds);

    /// <summary>
    /// Removes the specified <see cref="Led"/> from this <see cref="ListLedGroup"/>.
    /// </summary>
    /// <param name="leds">The <see cref="Led"/> to remove.</param>
    public void RemoveLeds(IEnumerable<Led> leds)
    {
        lock (_groupLeds)
            foreach (Led led in leds)
                _groupLeds.Remove(led);
    }

    /// <summary>
    /// Checks if a specified LED is contained by this ledgroup.
    /// </summary>
    /// <param name="led">The LED which should be checked.</param>
    /// <returns><c>true</c> if the LED is contained by this ledgroup; otherwise, <c>false</c>.</returns>
    public bool ContainsLed(Led led)
    {
        lock (_groupLeds)
            return _groupLeds.Contains(led);
    }

    /// <summary>
    /// Merges the <see cref="Led"/> from the specified ledgroup in this ledgroup. 
    /// </summary>
    /// <param name="groupToMerge">The ledgroup to merge.</param>
    public void MergeLeds(ILedGroup groupToMerge)
    {
        lock (_groupLeds)
            foreach (Led led in groupToMerge)
                if (!_groupLeds.Contains(led))
                    _groupLeds.Add(led);
    }

    /// <inheritdoc />
    /// <summary>
    /// Gets a list containing the <see cref="T:RGB.NET.Core.Led" /> from this group.
    /// </summary>
    /// <returns>The list containing the <see cref="T:RGB.NET.Core.Led" />.</returns>
    protected override IEnumerable<Led> GetLeds() => ToList();

    /// <inheritdoc />
    /// <summary>
    /// Gets a list containing the <see cref="T:RGB.NET.Core.Led" /> from this group.
    /// </summary>
    /// <returns>The list containing the <see cref="T:RGB.NET.Core.Led" />.</returns>
    public override IList<Led> ToList()
    {
        lock (_groupLeds)
            return [.._groupLeds];
    }

    protected override IDisposable ToListUnsafe(out IList<Led> leds)
    {
        Monitor.Enter(_groupLeds);
        leds = _groupLeds;
        return _unlockDisposable;
    }

    private void Unlock() => Monitor.Exit(_groupLeds);

    #endregion
}