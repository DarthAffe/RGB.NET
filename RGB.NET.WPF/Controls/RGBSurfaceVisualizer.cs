using System.Windows;
using System.Windows.Controls;
using RGB.NET.Core;

namespace RGB.NET.WPF.Controls
{
    /// <summary>
    /// Visualizes the <see cref="RGBSurface"/> in an wpf-application.
    /// </summary>
    [TemplatePart(Name = PART_CANVAS, Type = typeof(Canvas))]
    public class RGBSurfaceVisualizer : Control
    {
        #region Constants

        private const string PART_CANVAS = "PART_Canvas";

        #endregion

        #region Properties & Fields

        private Canvas _canvas;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBSurfaceVisualizer"/> class.
        /// </summary>
        public RGBSurfaceVisualizer()
        {
            RGBSurface.SurfaceLayoutChanged += RGBSurfaceOnSurfaceLayoutChanged;
            foreach (IRGBDevice device in RGBSurface.Devices)
                AddDevice(device);
        }

        private void RGBSurfaceOnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs args)
        {
            if (args.DeviceAdded)
                foreach (IRGBDevice device in args.Devices)
                    AddDevice(device);

            _canvas.Width = RGBSurface.SurfaceRectangle.Size.Width;
            _canvas.Height = RGBSurface.SurfaceRectangle.Size.Height;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _canvas = (Canvas)GetTemplateChild(PART_CANVAS);
        }

        private void AddDevice(IRGBDevice device)
        {
            _canvas.Children.Add(new RGBDeviceVisualizer { Device = device });
        }

        #endregion
    }
}
