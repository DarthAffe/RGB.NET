// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a basic effect-target.
    /// </summary>
    /// <typeparam name="T">The type this target represents.</typeparam>
    public interface IEffectTarget<T>
        where T : IEffectTarget<T>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a readonly collection of all <see cref="IEffect{T}"/> of this <see cref="IEffectTarget{T}"/>.
        /// </summary>
        IEnumerable<IEffect<T>> Effects { get; }

        #endregion

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

        /// <summary>
        /// Checks if the <see cref="IEffect{T}"/> is added to this <see cref="IEffectTarget{T}"/>.
        /// </summary>
        /// <param name="effect">The <see cref="IEffect{T}"/> to check.</param>
        /// <returns><c>true</c> if the <see cref="IEffect{T}"/> is added to this <see cref="IEffectTarget{T}"/>.; otherwise, <c>false</c>.</returns>
        bool HasEffect(IEffect<T> effect);

        /// <summary>
        /// Checks if any <see cref="IEffect{T}"/> of the provided generic type is added to this <see cref="IEffectTarget{T}"/>.
        /// </summary>
        /// <typeparam name="TEffect">The generic type of the <see cref="IEffect{T}"/> to check.</typeparam>
        /// <returns><c>true</c> if any <see cref="IEffect{T}"/> of the provided type is added to this <see cref="IEffectTarget{T}"/>.; otherwise, <c>false</c>.</returns>
        bool HasEffect<TEffect>() where TEffect : IEffect;

        #endregion
    }
}
