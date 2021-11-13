// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for <see cref="ILedGroup"/> related things.
/// </summary>
public static class LedGroupExtension
{
    /// <summary>
    /// Converts the specified <see cref="ILedGroup" /> to a <see cref="ListLedGroup" />.
    /// </summary>
    /// <param name="ledGroup">The <see cref="ILedGroup" /> to convert.</param>
    /// <returns>The converted <see cref="ListLedGroup" />.</returns>
    public static ListLedGroup ToListLedGroup(this ILedGroup ledGroup)
    {
        // ReSharper disable once InvertIf
        if (ledGroup is not ListLedGroup listLedGroup)
        {
            if (ledGroup.IsAttached)
                ledGroup.Detach();
            listLedGroup = new ListLedGroup(ledGroup.Surface, ledGroup) { Brush = ledGroup.Brush, ZIndex = ledGroup.ZIndex };
        }
        return listLedGroup;
    }

    /// <summary>
    /// Returns a new <see cref="ListLedGroup" /> which contains all <see cref="Led"/> from the specified <see cref="ILedGroup"/> excluding the specified ones.
    /// </summary>
    /// <param name="ledGroup">The base <see cref="ILedGroup"/>.</param>
    /// <param name="ledIds">The <see cref="Led"/> to exclude.</param>
    /// <returns>The new <see cref="ListLedGroup" />.</returns>
    public static ListLedGroup Exclude(this ILedGroup ledGroup, params Led[] ledIds)
    {
        ListLedGroup listLedGroup = ledGroup.ToListLedGroup();
        foreach (Led led in ledIds)
            listLedGroup.RemoveLed(led);
        return listLedGroup;
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    /// <summary>
    /// Attaches the specified <see cref="ILedGroup"/> to the <see cref="RGBSurface"/>.
    /// </summary>
    /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
    /// <param name="surface">The <see cref="RGBSurface"/> to attach this group to.</param>
    /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
    public static bool Attach(this ILedGroup ledGroup, RGBSurface surface) => surface.Attach(ledGroup);

    /// <summary>
    /// Detaches the specified <see cref="ILedGroup"/> from the <see cref="RGBSurface"/>.
    /// </summary>
    /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
    /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be detached; otherwise, <c>false</c>.</returns>
    public static bool Detach(this ILedGroup ledGroup) => ledGroup.Surface?.Detach(ledGroup) ?? false;
}