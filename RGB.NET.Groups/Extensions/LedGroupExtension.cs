// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Groups
{
    /// <summary>
    /// Offers some extensions and helper-methods for <see cref="ILedGroup"/> related things.
    /// </summary>
    public static class LedGroupExtension
    {
        /// <summary>
        /// Converts the given <see cref="ILedGroup" /> to a <see cref="ListLedGroup" />.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup" /> to convert.</param>
        /// <returns>The converted <see cref="ListLedGroup" />.</returns>
        public static ListLedGroup ToListLedGroup(this ILedGroup ledGroup)
        {
            ListLedGroup listLedGroup = ledGroup as ListLedGroup;
            // ReSharper disable once InvertIf
            if (listLedGroup == null)
            {
                bool wasAttached = ledGroup.Detach();
                listLedGroup = new ListLedGroup(wasAttached, ledGroup.GetLeds()) { Brush = ledGroup.Brush };
            }
            return listLedGroup;
        }

        /// <summary>
        /// Returns a new <see cref="ListLedGroup" /> which contains all <see cref="Led"/> from the given <see cref="ILedGroup"/> excluding the specified ones.
        /// </summary>
        /// <param name="ledGroup">The base <see cref="ILedGroup"/>.</param>
        /// <param name="ledIds">The ids of the <see cref="Led"/> to exclude.</param>
        /// <returns>The new <see cref="ListLedGroup" />.</returns>
        public static ListLedGroup Exclude(this ILedGroup ledGroup, params ILedId[] ledIds)
        {
            ListLedGroup listLedGroup = ledGroup.ToListLedGroup();
            foreach (ILedId ledId in ledIds)
                listLedGroup.RemoveLed(ledId);
            return listLedGroup;
        }

        /// <summary>
        /// Returns a new <see cref="ListLedGroup" /> which contains all <see cref="Led"/> from the given <see cref="ILedGroup"/> excluding the specified ones.
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
        /// Attaches the given <see cref="ILedGroup"/> to the <see cref="RGBSurface"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        public static bool Attach(this ILedGroup ledGroup)
        {
            return RGBSurface.Instance.AttachLedGroup(ledGroup);
        }

        /// <summary>
        /// Detaches the given <see cref="ILedGroup"/> from the <see cref="RGBSurface"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be detached; otherwise, <c>false</c>.</returns>
        public static bool Detach(this ILedGroup ledGroup)
        {
            return RGBSurface.Instance.DetachLedGroup(ledGroup);
        }
    }
}
