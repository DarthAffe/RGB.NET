// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// Represents a RGB-surface containing multiple devices.
    /// </summary>
    public partial class RGBSurface : AbstractBindable, IDisposable
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the singelot-instance of the <see cref="RGBSurface"/> class.
        /// </summary>
        public static RGBSurface Instance { get; } = new RGBSurface();

        private DateTime _lastUpdate;

        private IList<IRGBDeviceProvider> _deviceProvider = new List<IRGBDeviceProvider>();
        private IList<IRGBDevice> _devices = new List<IRGBDevice>();

        // ReSharper disable InconsistentNaming

        private readonly LinkedList<ILedGroup> _ledGroups = new LinkedList<ILedGroup>();

        private readonly Rectangle _surfaceRectangle = new Rectangle();

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Gets a readonly list containing all loaded <see cref="IRGBDevice"/>.
        /// </summary>
        public IEnumerable<IRGBDevice> Devices => new ReadOnlyCollection<IRGBDevice>(_devices);

        /// <summary>
        /// Gets a copy of the <see cref="Rectangle"/> representing this <see cref="RGBSurface"/>.
        /// </summary>
        public Rectangle SurfaceRectangle => new Rectangle(_surfaceRectangle);

        /// <summary>
        /// Gets a list of all <see cref="Led"/> on this <see cref="RGBSurface"/>.
        /// </summary>
        public IEnumerable<Led> Leds => _devices.SelectMany(x => x);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBSurface"/> class.
        /// </summary>
        private RGBSurface()
        {
            _lastUpdate = DateTime.Now;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform an update for all dirty <see cref="Led"/>, or all <see cref="Led"/>, if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all <see cref="Led"/>, (including clean ones) should be updated.</param>
        public void Update(bool flushLeds = false)
        {
            try
            {
                OnUpdating();

                lock (_ledGroups)
                {
                    // Render brushes
                    foreach (ILedGroup ledGroup in _ledGroups.OrderBy(x => x.ZIndex))
                        try { Render(ledGroup); }
                        catch (Exception ex) { OnException(ex); }
                }

                foreach (IRGBDevice device in Devices)
                    try { device.Update(flushLeds); }
                    catch (Exception ex) { OnException(ex); }

                OnUpdated();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_updateTokenSource?.IsCancellationRequested == false)
                _updateTokenSource.Cancel();

            foreach (IRGBDevice device in _devices)
                try { device.Dispose(); }
                catch { /* We do what we can */ }

            _ledGroups.Clear();
            _devices = null;
            _deviceProvider = null;
        }

        /// <summary>
        /// Renders a ledgroup.
        /// </summary>
        /// <param name="ledGroup">The led group to render.</param>
        private void Render(ILedGroup ledGroup)
        {
            IList<Led> leds = ledGroup.GetLeds().ToList();
            IBrush brush = ledGroup.Brush;

            if ((brush == null) || !brush.IsEnabled) return;

            switch (brush.BrushCalculationMode)
            {
                case BrushCalculationMode.Relative:
                    Rectangle brushRectangle = new Rectangle(leds.Select(x => GetDeviceLedLocation(x)));
                    Point offset = new Point(-brushRectangle.Location.X, -brushRectangle.Location.Y);
                    brushRectangle.Location.X = 0;
                    brushRectangle.Location.Y = 0;
                    brush.PerformRender(brushRectangle,
                                        leds.Select(x => new BrushRenderTarget(x, GetDeviceLedLocation(x, offset))));
                    break;
                case BrushCalculationMode.Absolute:
                    brush.PerformRender(SurfaceRectangle, leds.Select(x => new BrushRenderTarget(x, GetDeviceLedLocation(x))));
                    break;
                default:
                    throw new ArgumentException();
            }

            //brush.UpdateEffects();
            brush.PerformFinalize();

            foreach (KeyValuePair<BrushRenderTarget, Color> renders in brush.RenderedTargets)
                renders.Key.Led.Color = renders.Value;
        }

        private Rectangle GetDeviceLedLocation(Led led, Point extraOffset = null)
        {
            return extraOffset != null
                       ? new Rectangle(led.LedRectangle.Location + led.Device.Location + extraOffset, new Size(led.LedRectangle.Size.Width, led.LedRectangle.Size.Height))
                       : new Rectangle(led.LedRectangle.Location + led.Device.Location, new Size(led.LedRectangle.Size.Width, led.LedRectangle.Size.Height));
        }

        /// <summary>
        /// Attaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachLedGroup(ILedGroup ledGroup)
        {
            if (ledGroup == null) return false;

            lock (_ledGroups)
            {
                if (_ledGroups.Contains(ledGroup)) return false;

                _ledGroups.AddLast(ledGroup);
                ledGroup.OnAttach();

                return true;
            }
        }

        /// <summary>
        /// Detaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to detached.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be detached; otherwise, <c>false</c>.</returns>
        public bool DetachLedGroup(ILedGroup ledGroup)
        {
            if (ledGroup == null) return false;

            lock (_ledGroups)
            {
                LinkedListNode<ILedGroup> node = _ledGroups.Find(ledGroup);
                if (node == null) return false;

                _ledGroups.Remove(node);
                node.Value.OnDetach();

                return true;
            }
        }

        private void UpdateSurfaceRectangle()
        {
            Rectangle devicesRectangle = new Rectangle(_devices.Select(d => new Rectangle(d.Location, d.Size)));

            _surfaceRectangle.Size.Width = devicesRectangle.Location.X + devicesRectangle.Size.Width;
            _surfaceRectangle.Size.Height = devicesRectangle.Location.Y + devicesRectangle.Size.Height;
        }

        #endregion
    }
}
