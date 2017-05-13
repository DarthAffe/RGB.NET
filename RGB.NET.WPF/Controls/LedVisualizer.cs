using System.Windows;
using System.Windows.Controls;
using RGB.NET.Core;

namespace RGB.NET.WPF.Controls
{
    /// <summary>
    /// Visualizes a <see cref="Core.Led"/> in an wpf-application.
    /// </summary>
    public class LedVisualizer : Control
    {
        #region DependencyProperties
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Backing-property for the <see cref="Led"/>-property.
        /// </summary>
        public static readonly DependencyProperty LedProperty = DependencyProperty.Register(
            "Led", typeof(Led), typeof(LedVisualizer), new PropertyMetadata(default(Led)));

        /// <summary>
        /// Gets or sets the <see cref="Core.Led"/> to visualize.
        /// </summary>
        public Led Led
        {
            get => (Led)GetValue(LedProperty);
            set => SetValue(LedProperty, value);
        }

        // ReSharper restore InconsistentNaming
        #endregion
    }
}
