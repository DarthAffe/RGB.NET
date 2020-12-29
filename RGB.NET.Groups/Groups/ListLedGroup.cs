// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Groups
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a ledgroup containing arbitrary <see cref="T:RGB.NET.Core.Led" />.
    /// </summary>
    public class ListLedGroup : AbstractLedGroup
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the list containing the <see cref="Led"/> of this <see cref="ListLedGroup"/>.
        /// </summary>
        protected IList<Led> GroupLeds { get; } = new List<Led>();

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
        /// </summary>
        /// <param name="autoAttach">Specifies whether this <see cref="T:RGB.NET.Groups.ListLedGroup" /> should be automatically attached or not.</param>
        public ListLedGroup(RGBSurface? surface)
            : base(surface)
        { }
        
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
        /// </summary>
        /// <param name="autoAttach">Specifies whether this <see cref="T:RGB.NET.Groups.ListLedGroup" /> should be automatically attached or not.</param>
        /// <param name="leds">The initial <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Groups.ListLedGroup" />.</param>
        public ListLedGroup(RGBSurface? surface, IEnumerable<Led> leds)
            : base(surface)
        {
            AddLeds(leds);
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.ListLedGroup" /> class.
        /// </summary>
        /// <param name="autoAttach">Specifies whether this <see cref="T:RGB.NET.Groups.ListLedGroup" /> should be automatically attached or not.</param>
        /// <param name="leds">The initial <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Groups.ListLedGroup" />.</param>
        public ListLedGroup(RGBSurface? surface, params Led[] leds)
            : base(surface)
        {
            AddLeds(leds);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given LED(s) to this <see cref="ListLedGroup"/>.
        /// </summary>
        /// <param name="leds">The LED(s) to add.</param>
        public void AddLed(params Led[] leds) => AddLeds(leds);

        /// <summary>
        /// Adds the given <see cref="Led"/> to this <see cref="ListLedGroup"/>.
        /// </summary>
        /// <param name="leds">The <see cref="Led"/> to add.</param>
        public void AddLeds(IEnumerable<Led> leds)
        {
            if (leds == null) return;

            lock (GroupLeds)
                foreach (Led led in leds)
                    if ((led != null) && !ContainsLed(led))
                        GroupLeds.Add(led);
        }

        /// <summary>
        /// Removes the given LED(s) from this <see cref="ListLedGroup"/>.
        /// </summary>
        /// <param name="leds">The LED(s) to remove.</param>
        public void RemoveLed(params Led[] leds) => RemoveLeds(leds);

        /// <summary>
        /// Removes the given <see cref="Led"/> from this <see cref="ListLedGroup"/>.
        /// </summary>
        /// <param name="leds">The <see cref="Led"/> to remove.</param>
        public void RemoveLeds(IEnumerable<Led> leds)
        {
            if (leds == null) return;

            lock (GroupLeds)
                foreach (Led led in leds)
                    if (led != null)
                        GroupLeds.Remove(led);
        }

        /// <summary>
        /// Checks if a given LED is contained by this ledgroup.
        /// </summary>
        /// <param name="led">The LED which should be checked.</param>
        /// <returns><c>true</c> if the LED is contained by this ledgroup; otherwise, <c>false</c>.</returns>
        public bool ContainsLed(Led led)
        {
            lock (GroupLeds)
                return (led != null) && GroupLeds.Contains(led);
        }

        /// <summary>
        /// Merges the <see cref="Led"/> from the given ledgroup in this ledgroup. 
        /// </summary>
        /// <param name="groupToMerge">The ledgroup to merge.</param>
        public void MergeLeds(ILedGroup groupToMerge)
        {
            lock (GroupLeds)
                foreach (Led led in groupToMerge.GetLeds())
                    if (!GroupLeds.Contains(led))
                        GroupLeds.Add(led);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets a list containing the <see cref="T:RGB.NET.Core.Led" /> from this group.
        /// </summary>
        /// <returns>The list containing the <see cref="T:RGB.NET.Core.Led" />.</returns>
        public override IList<Led> GetLeds()
        {
            lock (GroupLeds)
                return new List<Led>(GroupLeds);
        }

        #endregion
    }
}
