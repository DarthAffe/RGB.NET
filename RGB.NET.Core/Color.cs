// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Diagnostics;
using RGB.NET.Core.Extensions;
using RGB.NET.Core.MVVM;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color.
    /// </summary>
    [DebuggerDisplay("[A: {A}, R: {R}, G: {G}, B: {B}, H: {Hue}, S: {Saturation}, V: {Value}]")]
    public class Color : AbstractBindable
    {
        #region Constants

        /// <summary>
        /// Gets an transparent color [A: 0, R: 0, G: 0, B: 0]
        /// </summary>
        public static Color Transparent => new Color();

        #endregion

        #region Properties & Fields

        #region RGB

        private byte _a;
        /// <summary>
        /// Gets or sets the alpha component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte A
        {
            get { return _a; }
            set
            {
                if (SetProperty(ref _a, value))
                    // ReSharper disable once ExplicitCallerInfoArgument
                    OnPropertyChanged(nameof(APercent));
            }
        }

        /// <summary>
        /// Gets or sets the alpha component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double APercent
        {
            get { return A / (double)byte.MaxValue; }
            set { A = GetByteValueFromPercentage(value); }
        }

        private byte _r;
        /// <summary>
        /// Gets or sets the red component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte R
        {
            get { return _r; }
            set
            {
                if (SetProperty(ref _r, value))
                {
                    InvalidateHSV();
                    // ReSharper disable once ExplicitCallerInfoArgument
                    OnPropertyChanged(nameof(RPercent));
                }
            }
        }

        /// <summary>
        /// Gets or sets the red component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double RPercent
        {
            get { return R / (double)byte.MaxValue; }
            set { R = GetByteValueFromPercentage(value); }
        }

        private byte _g;
        /// <summary>
        /// Gets or sets the green component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte G
        {
            get { return _g; }
            set
            {
                if (SetProperty(ref _g, value))
                {
                    InvalidateHSV();
                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged(nameof(GPercent));
                    // ReSharper restore ExplicitCallerInfoArgument
                }
            }
        }

        /// <summary>
        /// Gets or sets the green component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double GPercent
        {
            get { return G / (double)byte.MaxValue; }
            set { G = GetByteValueFromPercentage(value); }
        }

        private byte _b;
        /// <summary>
        /// Gets or sets the blue component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte B
        {
            get { return _b; }
            set
            {
                if (SetProperty(ref _b, value))
                {
                    InvalidateHSV();
                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged(nameof(BPercent));
                    // ReSharper restore ExplicitCallerInfoArgument
                }
            }
        }

        /// <summary>
        /// Gets or sets the blue component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double BPercent
        {
            get { return B / (double)byte.MaxValue; }
            set { B = GetByteValueFromPercentage(value); }
        }

        #endregion

        #region HSV

        private double? _hue;
        /// <summary>
        /// Gets or sets the hue component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..360].
        /// </summary>
        public double Hue
        {
            get { return _hue ?? (_hue = CaclulateHueFromRGB()).Value; }
            set
            {
                if (SetProperty(ref _hue, value))
                    UpdateRGBFromHSV();
            }
        }

        private double? _saturation;
        /// <summary>
        /// Gets or sets the saturation component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..1].
        /// </summary>
        public double Saturation
        {
            get { return _saturation ?? (_saturation = CaclulateSaturationFromRGB()).Value; }
            set
            {
                if (SetProperty(ref _saturation, value))
                    UpdateRGBFromHSV();
            }
        }

        private double? _value;
        /// <summary>
        /// Gets or sets the value component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..1].
        /// </summary>
        public double Value
        {
            get { return _value ?? (_value = CaclulateValueFromRGB()).Value; }
            set
            {
                if (SetProperty(ref _value, value))
                    UpdateRGBFromHSV();
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// The class created by this constructor equals <see cref="Transparent"/>.
        /// </summary>
        public Color()
            : this(0, 0, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class using only RGB-Values. 
        /// Alpha defaults to 255.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(byte r, byte g, byte b)
            : this(255, r, g, b)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class using ARGB-values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class using only RGB-Values. 
        /// Alpha defaults to 255.
        /// </summary>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        public Color(double hue, double saturation, double value)
            : this(255, hue, saturation, value)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class using ARGB-values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        public Color(byte a, double hue, double saturation, double value)
        {
            this.A = a;
            this._hue = hue;
            this._saturation = saturation;
            this._value = value;

            // ReSharper disable ExplicitCallerInfoArgument
            // ReSharper disable VirtualMemberCallInConstructor
            OnPropertyChanged(nameof(Hue));
            OnPropertyChanged(nameof(Saturation));
            OnPropertyChanged(nameof(Value));
            // ReSharper restore VirtualMemberCallInConstructor
            // ReSharper restore ExplicitCallerInfoArgument

            UpdateRGBFromHSV();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class by cloning a existing <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> the values are copied from.</param>
        public Color(Color color)
            : this(color.A, color.R, color.G, color.B)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Blends a <see cref="Color"/> over this color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to blend.</param>
        public void Blend(Color color)
        {
            if (color.A == 0) return;

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (color.A == 255)
            {
                A = color.A;
                R = color.A;
                G = color.A;
                B = color.A;
            }
            else
            {
                double resultA = (1.0 - ((1.0 - color.APercent) * (1.0 - APercent)));
                double resultR = (((color.RPercent * color.APercent) / resultA) + ((RPercent * APercent * (1.0 - color.APercent)) / resultA));
                double resultG = (((color.GPercent * color.APercent) / resultA) + ((GPercent * APercent * (1.0 - color.APercent)) / resultA));
                double resultB = (((color.BPercent * color.APercent) / resultA) + ((BPercent * APercent * (1.0 - color.APercent)) / resultA));

                APercent = resultA;
                RPercent = resultR;
                GPercent = resultG;
                BPercent = resultB;
            }
        }

        private void InvalidateHSV()
        {
            _hue = null;
            _saturation = null;
            _value = null;

            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Hue));
            OnPropertyChanged(nameof(Saturation));
            OnPropertyChanged(nameof(Value));
            // ReSharper restore ExplicitCallerInfoArgument
        }

        private double CaclulateHueFromRGB()
        {
            if ((R == G) && (G == B)) return 0.0;

            double min = Math.Min(Math.Min(R, G), B);
            double max = Math.Max(Math.Max(R, G), B);

            double hue;
            if (max.EqualsInTolerance(R)) // r is max
                hue = (G - B) / (max - min);
            else if (max.EqualsInTolerance(G)) // g is max
                hue = 2.0 + ((B - R) / (max - min));
            else // b is max
                hue = 4.0 + ((R - G) / (max - min));

            hue = hue * 60.0;
            if (hue < 0.0)
                hue += 360.0;

            return hue;
        }

        private double CaclulateSaturationFromRGB()
        {
            int max = Math.Max(R, Math.Max(G, B));
            int min = Math.Min(R, Math.Min(G, B));

            return (max == 0) ? 0 : 1.0 - (min / (double)max);
        }

        private double CaclulateValueFromRGB()
        {
            return Math.Max(R, Math.Max(G, B)) / 255.0;
        }

        private void UpdateRGBFromHSV()
        {
            if (Saturation <= 0.0)
            {
                byte val = GetByteValueFromPercentage(Value);
                UpdateRGBWithoutInvalidatingHSV(val, val, val);
            }

            double hh = (Hue % 360.0) / 60.0;
            int i = (int)hh;
            double ff = hh - i;
            double p = Value * (1.0 - Saturation);
            double q = Value * (1.0 - (Saturation * ff));
            double t = Value * (1.0 - (Saturation * (1.0 - ff)));

            switch (i)
            {
                case 0:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(Value),
                                                    GetByteValueFromPercentage(t),
                                                    GetByteValueFromPercentage(p));
                    break;
                case 1:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(q),
                                                    GetByteValueFromPercentage(Value),
                                                    GetByteValueFromPercentage(p));
                    break;
                case 2:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(p),
                                                    GetByteValueFromPercentage(Value),
                                                    GetByteValueFromPercentage(t));
                    break;
                case 3:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(p),
                                                    GetByteValueFromPercentage(q),
                                                    GetByteValueFromPercentage(Value));
                    break;
                case 4:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(t),
                                                    GetByteValueFromPercentage(p),
                                                    GetByteValueFromPercentage(Value));
                    break;
                default:
                    UpdateRGBWithoutInvalidatingHSV(GetByteValueFromPercentage(Value),
                                                    GetByteValueFromPercentage(p),
                                                    GetByteValueFromPercentage(q));
                    break;
            }
        }

        private void UpdateRGBWithoutInvalidatingHSV(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;

            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(RPercent));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(GPercent));
            OnPropertyChanged(nameof(B));
            OnPropertyChanged(nameof(BPercent));
            // ReSharper enable ExplicitCallerInfoArgument
        }

        private static byte GetByteValueFromPercentage(double percentage)
        {
            if (double.IsNaN(percentage)) return 0;

            percentage = percentage.Clamp(0, 1.0);
            return (byte)(percentage.Equals(1.0) ? 255 : percentage * 256.0);
        }

        /// <summary>
        /// Converts the individual byte-values of this <see cref="Color"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the individual byte-values of this <see cref="Color"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString()
        {
            return $"[A: {A}, R: {R}, G: {G}, B: {B}, H: {Hue}, S: {Saturation}, V: {Value}]";
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Color compareColor = obj as Color;
            if (ReferenceEquals(compareColor, null))
                return false;

            if (ReferenceEquals(this, compareColor))
                return true;

            if (GetType() != compareColor.GetType())
                return false;

            return (compareColor.A == A) && (compareColor.R == R) && (compareColor.G == G) && (compareColor.B == B);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Color" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = A.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="Color" /> to compare.</param>
        /// <param name="color2">The second <see cref="Color" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color color1, Color color2)
        {
            return ReferenceEquals(color1, null) ? ReferenceEquals(color2, null) : color1.Equals(color2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="Color" /> to compare.</param>
        /// <param name="color2">The second <see cref="Color" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color color1, Color color2)
        {
            return !(color1 == color2);
        }

        #endregion
    }
}
