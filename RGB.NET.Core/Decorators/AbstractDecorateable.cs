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

        private readonly List<T> _decorators = new List<T>();

        /// <inheritdoc />
        public IReadOnlyCollection<T> Decorators
        {
            get
            {
                lock (_decorators)
                    return new ReadOnlyCollection<T>(_decorators);
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void AddDecorator(T decorator)
        {
            lock (Decorators)
            {
                _decorators.Add(decorator);
                _decorators.Sort((d1, d2) => d1.Order.CompareTo(d2.Order));
            }

            decorator.OnAttached(this);
        }

        /// <inheritdoc />
        public void RemoveDecorator(T decorator)
        {
            lock (Decorators)
                _decorators.Remove(decorator);

            decorator.OnDetached(this);
        }

        /// <inheritdoc />
        public void RemoveAllDecorators()
        {
            IEnumerable<T> decorators;

            lock (Decorators)
                decorators = Decorators.ToList();

            foreach (T decorator in decorators)
                RemoveDecorator(decorator);
        }

        #endregion
    }
}
