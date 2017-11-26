using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractDecoratable{T}" />
    /// <inheritdoc cref="ILedGroup" />
    /// <summary>
    /// Represents a generic <see cref="T:RGB.NET.Core.AbstractLedGroup" />.
    /// </summary>
    public abstract class AbstractLedGroup : AbstractDecoratable<ILedGroupDecorator>, ILedGroup
    {
        #region Properties & Fields

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
