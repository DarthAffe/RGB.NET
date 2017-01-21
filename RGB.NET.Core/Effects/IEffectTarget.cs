// ReSharper disable UnusedMember.Global

namespace RGB.NET.Core.Effects
{
    /// <summary>
    /// Represents a basic effect-target.
    /// </summary>
    /// <typeparam name="T">The type this target represents.</typeparam>
    public interface IEffectTarget<out T>
        where T : IEffectTarget<T>
    {
        #region Methods

        /// <summary>
        /// Updates all <see cref="IEffect{T}"/> added to this target.
        /// </summary>
        void UpdateEffects();

        /// <summary>
        /// Adds an <see cref="IEffect{T}"/>.
        /// </summary>
        /// <param name="effect">The <see cref="IEffect{T}"/> to add.</param>
        void AddEffect(IEffect<T> effect);

        /// <summary>
        /// Removes an <see cref="IEffect{T}"/>.
        /// </summary>
        /// <param name="effect">The <see cref="IEffect{T}"/> to remove.</param>
        void RemoveEffect(IEffect<T> effect);

        #endregion
    }
}
