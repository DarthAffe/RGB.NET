using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IDecoratable{T}" />
    public abstract class AbstractDecoratable<T> : AbstractBindable, IDecoratable<T>
        where T : IDecorator
    {
        #region Properties & Fields

        private List<T> _decorators = new List<T>();
        /// <summary>
        /// Gets a readonly-list of all <see cref="IDecorator"/> attached to this <see cref="IDecoratable{T}"/>.
        /// </summary>
        protected IReadOnlyCollection<T> Decorators => new ReadOnlyCollection<T>(_decorators);

        #endregion

        #region Methods

        /// <inheritdoc />
        public void AddDecorator(T decorator)
        {
            _decorators.Add(decorator);
            _decorators = _decorators.OrderByDescending(x => x.Order).ToList();

            decorator.OnAttached(this);
        }

        /// <inheritdoc />
        public void RemoveDecorator(T decorator)
        {
            _decorators.Remove(decorator);

            decorator.OnDetached(this);
        }

        /// <inheritdoc />
        public void RemoveAllDecorators()
        {
            foreach (T decorator in Decorators.ToList())
                RemoveDecorator(decorator);
        }

        #endregion
    }
}
