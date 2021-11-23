using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core;

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
    protected List<IDecoratable> DecoratedObjects { get; } = new();

    #endregion

    #region Methods

    /// <inheritdoc />
    public virtual void OnAttached(IDecoratable decoratable) => DecoratedObjects.Add(decoratable);

    /// <inheritdoc />
    public virtual void OnDetached(IDecoratable decoratable) => DecoratedObjects.Remove(decoratable);

    /// <summary>
    /// Detaches the decorator from all <see cref="IDecoratable"/> it is currently attached to.
    /// </summary>
    protected virtual void Detach()
    {
        List<IDecoratable> decoratables = new(DecoratedObjects);
        foreach (IDecoratable decoratable in decoratables)
        {
            IEnumerable<Type> types = decoratable.GetType().GetInterfaces().Where(t => t.IsGenericType
                                                                                    && (t.Name == typeof(IDecoratable<>).Name)
                                                                                    && t.GenericTypeArguments[0].IsInstanceOfType(this));
            foreach (Type decoratableType in types)
                decoratableType.GetMethod(nameof(IDecoratable<IDecorator>.RemoveDecorator))?.Invoke(decoratable, new object[] { this });
        }
    }

    #endregion
}