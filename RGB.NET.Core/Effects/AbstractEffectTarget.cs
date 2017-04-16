// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core.Exceptions;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents an generic effect-target.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractEffectTarget<T> : IEffectTarget<T>
        where T : IEffectTarget<T>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="EffectTimeContainer"/> storing the attached effects.
        /// </summary>
        protected IList<EffectTimeContainer> EffectTimes { get; } = new List<EffectTimeContainer>();

        /// <summary>
        /// Gets all <see cref="IEffect{T}" /> attached to this <see cref="IEffectTarget{T}"/>.
        /// </summary>
        protected IList<IEffect<T>> InternalEffects => EffectTimes.Select(x => x.Effect).Cast<IEffect<T>>().ToList();

        /// <inheritdoc />
        public IEnumerable<IEffect<T>> Effects => new ReadOnlyCollection<IEffect<T>>(InternalEffects);

        /// <summary>
        /// Gets the strongly-typed target used for the <see cref="IEffect{T}"/>.
        /// </summary>
        protected abstract T EffectTarget { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates all <see cref="IEffect"/> added to this <see cref="IEffectTarget{T}"/>.
        /// </summary>
        public virtual void UpdateEffects()
        {
            lock (InternalEffects)
            {
                for (int i = EffectTimes.Count - 1; i >= 0; i--)
                {
                    EffectTimeContainer effectTime = EffectTimes[i];
                    if (!effectTime.Effect.IsEnabled) continue;

                    long currentTicks = DateTime.Now.Ticks;

                    double deltaTime;
                    if (effectTime.TicksAtLastUpdate < 0)
                    {
                        effectTime.TicksAtLastUpdate = currentTicks;
                        deltaTime = 0;
                    }
                    else
                        deltaTime = (currentTicks - effectTime.TicksAtLastUpdate) / 10000000.0;

                    effectTime.TicksAtLastUpdate = currentTicks;
                    effectTime.Effect.Update(deltaTime);

                    if (effectTime.Effect.IsDone)
                        EffectTimes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adds an <see cref="IEffect{T}"/>.
        /// </summary>
        /// <param name="effect">The <see cref="IEffect{T}"/> to add.</param>
        public virtual void AddEffect(IEffect<T> effect)
        {
            if (EffectTimes.Any(x => x.Effect == effect)) return;

            if (!effect.CanBeAppliedTo(EffectTarget))
                throw new EffectException($"Failed to add effect.\r\n" +
                                          $"The effect of type '{effect.GetType()}' can't be applied to the target of type '{EffectTarget.GetType()}'.");

            effect.OnAttach(EffectTarget);
            EffectTimes.Add(new EffectTimeContainer(effect, -1));
        }

        /// <summary>
        /// Removes an <see cref="IEffect{T}"/>.
        /// </summary>
        /// <param name="effect">The <see cref="IEffect{T}"/> to remove.</param>
        public virtual void RemoveEffect(IEffect<T> effect)
        {
            EffectTimeContainer effectTimeToRemove = EffectTimes.FirstOrDefault(x => x.Effect == effect);
            if (effectTimeToRemove == null) return;

            effect.OnDetach(EffectTarget);
            EffectTimes.Remove(effectTimeToRemove);
        }

        /// <inheritdoc />
        public bool HasEffect(IEffect<T> effect)
        {
            return InternalEffects.Contains(effect);
        }

        /// <inheritdoc />
        public bool HasEffect<TEffect>()
            where TEffect : IEffect
        {
            return InternalEffects.Any(x => x.GetType() == typeof(TEffect));
        }

        #endregion
    }
}
