using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IDecorator" />
    public abstract class AbstractDecorator : AbstractBindable, IDecorator
    {
        #region Properties & Fields

        private bool _isEnabled = true;
        /// <inheritdoc />
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private int _order;
        /// <inheritdoc />
        public int Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        /// <summary>
        /// Gets a readonly-list of all <see cref="IDecoratable"/> this decorator is attached to.
        /// </summary>
        protected List<IDecoratable> DecoratedObjects { get; } = new List<IDecoratable>();

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void OnAttached(IDecoratable decoratable) => DecoratedObjects.Add(decoratable);

        /// <inheritdoc />
        public virtual void OnDetached(IDecoratable decoratable) => DecoratedObjects.Remove(decoratable);

        /// <summary>
        /// Detaches the decorator from all <see cref="IDecoratable"/> it is currently attached to.
        /// </summary>
        /// <typeparam name="TDecoratable">The type of the <see cref="IDecoratable"/> this decorator is attached to.</typeparam>
        /// <typeparam name="TDecorator">The type of this <see cref="IDecorator"/>.</typeparam>
        protected virtual void Detach<TDecoratable, TDecorator>()
            where TDecoratable : IDecoratable<TDecorator>
            where TDecorator : AbstractDecorator
        {
            List<IDecoratable> decoratables = new List<IDecoratable>(DecoratedObjects);
            foreach (IDecoratable decoratable in decoratables)
                if (decoratable is TDecoratable typedDecoratable)
                    typedDecoratable.RemoveDecorator((TDecorator)this);
        }

        #endregion
    }
}
