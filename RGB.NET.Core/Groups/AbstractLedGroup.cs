using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic <see cref="AbstractLedGroup"/>.
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLedGroup"/> class.
        /// </summary>
        /// <param name="autoAttach">Specifies whether this <see cref="AbstractLedGroup"/> should be automatically attached or not.</param>
        protected AbstractLedGroup(bool autoAttach)
        {
            if (autoAttach)
                RGBSurface.Instance.AttachLedGroup(this);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract IEnumerable<Led> GetLeds();

        /// <inheritdoc />
        public virtual void OnAttach()
        { }

        /// <inheritdoc />
        public virtual void OnDetach()
        { }

        #endregion
    }
}
