// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic effect targeting an <see cref="ILedGroup"/>.
    /// </summary>
    public abstract class AbstractLedGroupEffect<T> : IEffect<ILedGroup>
        where T : ILedGroup
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool IsDone { get; protected set; }

        /// <summary>
        /// Gets the <see cref="ILedGroup"/> this effect is targeting.
        /// </summary>
        protected T LedGroup { get; set; }

        #endregion

        #region Methods
        
        /// <inheritdoc />
        public abstract void Update(double deltaTime);

        /// <inheritdoc />
        public virtual bool CanBeAppliedTo(ILedGroup target)
        {
            return target is T;
        }

        /// <inheritdoc />
        public virtual void OnAttach(ILedGroup target)
        {
            LedGroup = (T)target;
        }

        /// <inheritdoc />
        public virtual void OnDetach(ILedGroup target)
        {
            LedGroup = default(T);
        }

        #endregion
    }

    /// <summary>
    /// Represents a basic effect targeting an <see cref="ILedGroup"/>.
    /// </summary>
    public abstract class AbstractLedGroupEffect : AbstractLedGroupEffect<ILedGroup>
    { }
}
