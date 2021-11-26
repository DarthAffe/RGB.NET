namespace RGB.NET.Core;

/// <summary>
/// Represents a basic decorator.
/// </summary>
public interface IDecorator
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets if the <see cref="IDecorator"/> is enabled and will be used.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the order in which multiple decorators should be applied on the same object.
    /// Higher orders are processed first.
    /// </summary>
    int Order { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Attaches this <see cref="IDecorator"/> to the specified target.
    /// </summary>
    /// <param name="decoratable">The object this <see cref="IDecorator"/> should be attached to.</param>
    void OnAttached(IDecoratable decoratable);

    /// <summary>
    /// Detaches this <see cref="IDecorator"/> from the specified target.
    /// </summary>
    /// <param name="decoratable">The object this <see cref="IDecorator"/> should be detached from.</param>
    void OnDetached(IDecoratable decoratable);

    #endregion
}