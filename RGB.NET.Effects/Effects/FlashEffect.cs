// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Core;

namespace RGB.NET.Effects
{
    /// <summary>
    /// Represents an effect which allows to flash an brush by modifying his opacity.
    /// </summary>
    public class FlashEffect : AbstractBrushEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the attack-time (in seconds) of the effect. (default: 0.2)<br />
        /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" />  as reference)
        /// </summary>
        public double Attack { get; set; } = 0.2;

        /// <summary>
        /// Gets or sets the decay-time (in seconds) of the effect. (default: 0)<br />
        /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
        /// </summary>
        public double Decay { get; set; } = 0;

        /// <summary>
        /// Gets or sets the sustain-time (in seconds) of the effect. (default: 0.3)<br />
        /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)<br />
        /// Note that this value for naming reasons represents the time NOT the level.
        /// </summary>
        public double Sustain { get; set; } = 0.3;

        /// <summary>
        /// Gets or sets the release-time (in seconds) of the effect. (default: 0.2)<br />
        /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
        /// </summary>
        public double Release { get; set; } = 0.2;

        /// <summary>
        /// Gets or sets the level to which the oppacity (percentage) should raise in the attack-cycle. (default: 1);
        /// </summary>
        public double AttackValue { get; set; } = 1;

        /// <summary>
        /// Gets or sets the level at which the oppacity (percentage) should stay in the sustain-cycle. (default: 1);
        /// </summary>
        public double SustainValue { get; set; } = 1;

        /// <summary>
        /// Gets or sets the interval (in seconds) in which the effect should repeat (if repetition is enabled). (default: 1)
        /// </summary>
        public double Interval { get; set; } = 1;

        /// <summary>
        /// Gets or sets the amount of repetitions the effect should do until it's finished. Zero means infinite. (default: 0)
        /// </summary>
        public int Repetitions { get; set; } = 0;

        private ADSRPhase _currentPhase;
        private double _currentPhaseValue;
        private int _repetitionCount;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Update(double deltaTime)
        {
            _currentPhaseValue -= deltaTime;

            // Using ifs instead of a switch allows to skip phases with time 0.
            // ReSharper disable InvertIf

            if (_currentPhase == ADSRPhase.Attack)
                if (_currentPhaseValue > 0)
                    Brush.Opacity = Math.Min(1, (Attack - _currentPhaseValue) / Attack) * AttackValue;
                else
                {
                    _currentPhaseValue = Decay;
                    _currentPhase = ADSRPhase.Decay;
                }

            if (_currentPhase == ADSRPhase.Decay)
                if (_currentPhaseValue > 0)
                    Brush.Opacity = SustainValue + (Math.Min(1, _currentPhaseValue / Decay) * (AttackValue - SustainValue));
                else
                {
                    _currentPhaseValue = Sustain;
                    _currentPhase = ADSRPhase.Sustain;
                }

            if (_currentPhase == ADSRPhase.Sustain)
                if (_currentPhaseValue > 0)
                    Brush.Opacity = SustainValue;
                else
                {
                    _currentPhaseValue = Release;
                    _currentPhase = ADSRPhase.Release;
                }

            if (_currentPhase == ADSRPhase.Release)
                if (_currentPhaseValue > 0)
                    Brush.Opacity = Math.Min(1, _currentPhaseValue / Release) * SustainValue;
                else
                {
                    _currentPhaseValue = Interval;
                    _currentPhase = ADSRPhase.Pause;
                }

            if (_currentPhase == ADSRPhase.Pause)
                if (_currentPhaseValue > 0)
                    Brush.Opacity = 0;
                else
                {
                    if ((++_repetitionCount >= Repetitions) && (Repetitions > 0))
                        IsDone = true;
                    _currentPhaseValue = Attack;
                    _currentPhase = ADSRPhase.Attack;
                }

            // ReSharper restore InvertIf
        }

        /// <summary>
        /// Resets the effect.
        /// </summary>
        public override void OnAttach(IBrush brush)
        {
            base.OnAttach(brush);

            _currentPhase = ADSRPhase.Attack;
            _currentPhaseValue = Attack;
            _repetitionCount = 0;
            brush.Opacity = 0;
        }

        #endregion

        // ReSharper disable once InconsistentNaming
        private enum ADSRPhase
        {
            Attack,
            Decay,
            Sustain,
            Release,
            Pause
        }
    }
}
