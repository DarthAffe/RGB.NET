// ReSharper disable MemberCanBePrivate.Global

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic effect targeting an <see cref="IBrush"/>.
    /// </summary>
    public abstract class AbstractBrushEffect<T> : IEffect<IBrush>
        where T : IBrush
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool IsDone { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IBrush"/> this effect is targeting.
        /// </summary>
        protected T Brush { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract void Update(double deltaTime);

        /// <inheritdoc />
        public virtual bool CanBeAppliedTo(IBrush target)
        {
            return target is T;
        }

        /// <inheritdoc />
        public virtual void OnAttach(IBrush target)
        {
            Brush = (T)target;
        }

        /// <inheritdoc />
        public virtual void OnDetach(IBrush target)
        {
            Brush = default(T);
        }

        #endregion
    }

    /// <summary>
    /// Represents a basic effect targeting an <see cref="IBrush"/>.
    /// </summary>
    public abstract class AbstractBrushEffect : AbstractBrushEffect<IBrush>
    { }
}
