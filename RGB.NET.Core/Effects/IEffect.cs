// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Global

using RGB.NET.Core.Devices;

namespace RGB.NET.Core.Effects
{
    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    public interface IEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets if this <see cref="IEffect"/> has finished all of his work.
        /// </summary>
        bool IsDone { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates this <see cref="IEffect"/>.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        void Update(float deltaTime);

        #endregion
    }

    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IEffectTarget{T}"/> this effect can be attached to.</typeparam>
    public interface IEffect<in T> : IEffect
        where T : IEffectTarget<T>
    {
        #region Methods

        /// <summary>
        /// Checks if the <see cref="IEffect{T}"/> can be applied to the target object.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is attached to.</param>
        /// <returns><c>true</c> if the <see cref="IEffectTarget{T}"/> can be attached; otherwise, <c>false</c>.</returns>
        bool CanBeAppliedTo(T target);

        /// <summary>
        /// Hook which is called when the <see cref="IEffect{T}"/> is attached to a <see cref="IRGBDevice"/>.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is attached to.</param>
        void OnAttach(T target);

        /// <summary>
        /// Hook which is called when the <see cref="IEffect{T}"/>  is detached from a <see cref="IRGBDevice"/>.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is detached from.</param>
        void OnDetach(T target);

        #endregion
    }
}
