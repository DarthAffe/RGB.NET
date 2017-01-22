// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents the information supplied with an <see cref="IRGBSurface.LedsUpdating"/>-event.
    /// </summary>
    public class LedsUpdatingEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="Led"/> which are about to get updated.
        /// </summary>
        public ICollection<Led> UpdatingLeds { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LedsUpdatingEventArgs"/> class.
        /// </summary>
        /// <param name="updatingLeds">The updating <see cref="Led"/>.</param>
        public LedsUpdatingEventArgs(ICollection<Led> updatingLeds)
        {
            this.UpdatingLeds = updatingLeds;
        }

        #endregion
    }
}
