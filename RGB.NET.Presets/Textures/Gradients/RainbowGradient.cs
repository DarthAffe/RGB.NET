// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using RGB.NET.Core;
using RGB.NET.Presets.Decorators;

namespace RGB.NET.Presets.Textures.Gradients;

/// <inheritdoc cref="AbstractDecoratable{T}" />
/// <inheritdoc cref="IGradient" />
/// <summary>
/// Represents a rainbow gradient which circles through all colors of the HUE-color-space.<br />
/// See <see href="http://upload.wikimedia.org/wikipedia/commons/a/ad/HueScale.svg" /> as reference.
/// </summary>
public class RainbowGradient : AbstractDecoratable<IGradientDecorator>, IGradient
{
    #region Properties & Fields

    private float _startHue;
    /// <summary>
    /// Gets or sets the hue (in degrees) to start from.
    /// </summary>
    public float StartHue
    {
        get => _startHue;
        set => SetProperty(ref _startHue, value);
    }

    private float _endHue;
    /// <summary>
    /// Gets or sets the hue (in degrees) to end the with.
    /// </summary>
    public float EndHue
    {
        get => _endHue;
        set => SetProperty(ref _endHue, value);
    }

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler? GradientChanged;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RainbowGradient"/> class.
    /// </summary>
    /// <param name="startHue">The hue (in degrees) to start from (default: 0)</param>
    /// <param name="endHue">The hue (in degrees) to end with (default: 360)</param>
    public RainbowGradient(float startHue = 0, float endHue = 360)
    {
        this.StartHue = startHue;
        this.EndHue = endHue;

        PropertyChanged += (_, _) => OnGradientChanged();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    /// <summary>
    /// Gets the color on the rainbow at the specified offset.
    /// </summary>
    /// <param name="offset">The percentage offset to take the color from.</param>
    /// <returns>The color at the specific offset.</returns>
    public Color GetColor(float offset)
    {
        float range = EndHue - StartHue;
        float hue = StartHue + (range * offset);
        return HSVColor.Create(hue, 1, 1);
    }

    /// <inheritdoc />
    public void Move(float offset)
    {
        // RainbowGradient is calculated inverse
        offset *= -1;

        StartHue += offset;
        EndHue += offset;

        while ((StartHue > 360) && (EndHue > 360))
        {
            StartHue -= 360;
            EndHue -= 360;
        }
        while ((StartHue < -360) && (EndHue < -360))
        {
            StartHue += 360;
            EndHue += 360;
        }
    }

    /// <summary>
    /// Should be called to indicate that the gradient was changed.
    /// </summary>
    protected void OnGradientChanged() => GradientChanged?.Invoke(this, EventArgs.Empty);

    #endregion
}