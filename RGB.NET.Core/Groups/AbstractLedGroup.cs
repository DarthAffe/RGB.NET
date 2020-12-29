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

        public RGBSurface? Surface { get; private set; }

        /// <inheritdoc />
        public IBrush Brush { get; set; }

        /// <inheritdoc />
        public int ZIndex { get; set; } = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLedGroup"/> class.
        /// </summary>
        protected AbstractLedGroup(RGBSurface? attachTo)
        {
            attachTo?.AttachLedGroup(this);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract IList<Led> GetLeds();

        /// <inheritdoc />
        public virtual void OnAttach(RGBSurface surface)
        {
            Surface = surface;
        }

        /// <inheritdoc />
        public virtual void OnDetach(RGBSurface surface)
        {
            Surface = null;
        }

        #endregion
    }
}
