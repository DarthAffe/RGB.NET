// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents the information supplied with an <see cref="IRGBSurface.LedsUpdated"/>-event.
    /// </summary>
    public class LedsUpdatedEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="Led"/> which got updated.
        /// </summary>
        public IEnumerable<Led> UpdatedLeds { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LedsUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="updatedLeds">The updated <see cref="Led"/>.</param>
        public LedsUpdatedEventArgs(IEnumerable<Led> updatedLeds)
        {
            this.UpdatedLeds = updatedLeds;
        }

        #endregion
    }
}
