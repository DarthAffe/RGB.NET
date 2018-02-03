using System.ComponentModel;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic decoratable.
    /// </summary>
    public interface IDecoratable : INotifyPropertyChanged
    { }

    /// <inheritdoc />
    /// <summary>
    /// Represents a basic decoratable for a specific type of <see cref="T:RGB.NET.Core.IDecorator" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDecoratable<in T> : IDecoratable
        where T : IDecorator
    {
        /// <summary>
        /// Adds an <see cref="IDecorator"/> to the <see cref="IDecoratable"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator"/> to be added.</param>
        void AddDecorator(T decorator);

        /// <summary>
        /// Removes an <see cref="IDecorator"/> from the <see cref="IDecoratable"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator"/> to be removed.</param>
        void RemoveDecorator(T decorator);

        /// <summary>
        /// Removes all <see cref="IDecorator"/> from the <see cref="IDecoratable"/>.
        /// </summary>
        void RemoveAllDecorators();
    }
}
