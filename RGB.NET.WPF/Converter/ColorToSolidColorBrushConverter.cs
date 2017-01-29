using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RGB.NET.WPF.Converter
{
    /// <summary>
    /// Converts <see cref="Core.Color"/> into <see cref="SolidColorBrush"/>.
    /// </summary>
    [ValueConversion(typeof(Core.Color), typeof(SolidColorBrush))]
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Core.Color color = value as Core.Color;
            return new SolidColorBrush(color == null
                ? Color.FromArgb(0, 0, 0, 0)
                : Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            return brush == null
                       ? Core.Color.Transparent
                       : new Core.Color(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
        }
    }
}
