using System.Collections.Generic;
using System.ComponentModel;

namespace RGB.NET.Core;

/// <summary>
/// Represents a basic decoratable.
/// </summary>
public interface IDecoratable : INotifyPropertyChanged
{ }

/// <inheritdoc />
/// <summary>
/// Represents a basic decoratable for a specific type of <see cref="T:RGB.NET.Core.IDecorator" />
/// </summary>
/// <typeparam name="T">The type of decorators this decoratable can be decorated with.</typeparam>
public interface IDecoratable<T> : IDecoratable
    where T : IDecorator
{
    /// <summary>
    /// Gets a readonly-list of all <see cref="IDecorator"/> attached to this <see cref="IDecoratable{T}"/>.
    /// </summary>
    IReadOnlyList<T> Decorators { get; }

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