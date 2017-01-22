// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a wrapped effect with additional time information.
    /// </summary>
    public class EffectTimeContainer
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the wrapped <see cref="IEffect"/>.
        /// </summary>
        public IEffect Effect { get; }

        /// <summary>
        /// Gets or sets the tick-count from the last time the <see cref="IEffect"/> was updated.
        /// </summary>
        public long TicksAtLastUpdate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectTimeContainer"/> class.
        /// </summary>
        /// <param name="effect">The wrapped <see cref="IEffect"/>.</param>
        /// <param name="ticksAtLastUpdate">The tick-count from the last time the <see cref="IEffect"/> was updated.</param>
        public EffectTimeContainer(IEffect effect, long ticksAtLastUpdate)
        {
            this.Effect = effect;
            this.TicksAtLastUpdate = ticksAtLastUpdate;
        }

        #endregion
    }
}
