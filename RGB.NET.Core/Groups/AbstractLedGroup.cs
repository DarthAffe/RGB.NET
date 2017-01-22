using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic ledgroup.
    /// </summary>
    public abstract class AbstractLedGroup : AbstractEffectTarget<ILedGroup>, ILedGroup
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the strongly-typed target used for the effect.
        /// </summary>
        protected override ILedGroup EffectTarget => this;

        /// <inheritdoc />
        public IBrush Brush { get; set; }

        /// <inheritdoc />
        public int ZIndex { get; set; } = 0;

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract IEnumerable<Led> GetLeds();

        #endregion
    }
}
