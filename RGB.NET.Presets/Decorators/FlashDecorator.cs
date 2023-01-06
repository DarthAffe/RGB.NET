// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Core;

namespace RGB.NET.Presets.Decorators;

/// <inheritdoc cref="AbstractUpdateAwareDecorator" />
/// <inheritdoc cref="IBrushDecorator" />
/// <summary>
/// Represents a decorator which allows to flash a brush by modifying his opacity.
/// </summary>
public class FlashDecorator : AbstractUpdateAwareDecorator, IBrushDecorator
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets the attack-time (in seconds) of the decorator. (default: 0.2)<br />
    /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" />  as reference)
    /// </summary>
    public float Attack { get; set; } = 0.2f;

    /// <summary>
    /// Gets or sets the decay-time (in seconds) of the decorator. (default: 0)<br />
    /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
    /// </summary>
    public float Decay { get; set; } = 0;

    /// <summary>
    /// Gets or sets the sustain-time (in seconds) of the decorator. (default: 0.3)<br />
    /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)<br />
    /// Note that this value for naming reasons represents the time NOT the level.
    /// </summary>
    public float Sustain { get; set; } = 0.3f;

    /// <summary>
    /// Gets or sets the release-time (in seconds) of the decorator. (default: 0.2)<br />
    /// This is close to a synthesizer envelope. (See <see href="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
    /// </summary>
    public float Release { get; set; } = 0.2f;

    /// <summary>
    /// Gets or sets the level to which the oppacity (percentage) should raise in the attack-cycle. (default: 1);
    /// </summary>
    public float AttackValue { get; set; } = 1;

    /// <summary>
    /// Gets or sets the level at which the oppacity (percentage) should stay in the sustain-cycle. (default: 1);
    /// </summary>
    public float SustainValue { get; set; } = 1;

    /// <summary>
    /// Gets or sets the level at which the oppacity (percentage) should stay in the pause-cycle. (default: 0);
    /// </summary>
    public float PauseValue { get; set; } = 0;

    /// <summary>
    /// Gets or sets the interval (in seconds) in which the decorator should repeat (if repetition is enabled). (default: 1)
    /// </summary>
    public float Interval { get; set; } = 1;

    /// <summary>
    /// Gets or sets the amount of repetitions the decorator should do until it's finished. Zero means infinite. (default: 0)
    /// </summary>
    public int Repetitions { get; set; } = 0;

    private ADSRPhase _currentPhase;
    private float _currentPhaseValue;
    private int _repetitionCount;

    private float _currentValue;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="FlashDecorator"/> from the specified xml.
    /// </summary>
    /// <param name="surface">The surface this decorator belongs to.</param>
    /// <param name="updateIfDisabled">A value indicating if the decorator should be updated if it is disabled.</param>
    public FlashDecorator(RGBSurface surface, bool updateIfDisabled = false)
        : base(surface, updateIfDisabled)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void ManipulateColor(in Rectangle rectangle, in RenderTarget renderTarget, ref Color color) => color = color.SetA(_currentValue);

    /// <inheritdoc />
    protected override void Update(double deltaTime)
    {
        _currentPhaseValue -= (float)deltaTime;

        // Using ifs instead of a switch allows to skip phases with time 0.
        // ReSharper disable InvertIf

        if (_currentPhase == ADSRPhase.Attack)
            if (_currentPhaseValue > 0)
                _currentValue = PauseValue + (MathF.Min(1, (Attack - _currentPhaseValue) / Attack) * (AttackValue - PauseValue));
            else
            {
                _currentPhaseValue = Decay;
                _currentPhase = ADSRPhase.Decay;
            }

        if (_currentPhase == ADSRPhase.Decay)
            if (_currentPhaseValue > 0)
                _currentValue = SustainValue + (MathF.Min(1, _currentPhaseValue / Decay) * (AttackValue - SustainValue));
            else
            {
                _currentPhaseValue = Sustain;
                _currentPhase = ADSRPhase.Sustain;
            }

        if (_currentPhase == ADSRPhase.Sustain)
            if (_currentPhaseValue > 0)
                _currentValue = SustainValue;
            else
            {
                _currentPhaseValue = Release;
                _currentPhase = ADSRPhase.Release;
            }

        if (_currentPhase == ADSRPhase.Release)
            if (_currentPhaseValue > 0)
                _currentValue = PauseValue + (MathF.Min(1, _currentPhaseValue / Release) * (SustainValue - PauseValue));
            else
            {
                _currentPhaseValue = Interval;
                _currentPhase = ADSRPhase.Pause;
            }

        if (_currentPhase == ADSRPhase.Pause)
            if (_currentPhaseValue > 0)
                _currentValue = PauseValue;
            else
            {
                if ((++_repetitionCount >= Repetitions) && (Repetitions > 0))
                    Detach();
                _currentPhaseValue = Attack;
                _currentPhase = ADSRPhase.Attack;
            }

        // ReSharper restore InvertIf
    }

    /// <inheritdoc cref="AbstractUpdateAwareDecorator.OnAttached" />
    /// <inheritdoc cref="IDecorator.OnAttached" />
    public override void OnAttached(IDecoratable decoratable)
    {
        base.OnAttached(decoratable);

        _currentPhase = ADSRPhase.Attack;
        _currentPhaseValue = Attack;
        _repetitionCount = 0;
        _currentValue = 0;
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