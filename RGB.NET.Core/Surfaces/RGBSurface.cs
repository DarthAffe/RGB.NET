using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic RGB-surface.
    /// </summary>
    public class RGBSurface : AbstractLedGroup, IRGBSurface
    {
        #region Properties & Fields

        private DateTime _lastUpdate;

        /// <inheritdoc />
        public Dictionary<IRGBDevice, Point> Devices { get; } = new Dictionary<IRGBDevice, Point>();

        private readonly LinkedList<ILedGroup> _ledGroups = new LinkedList<ILedGroup>();

        /// <inheritdoc />
        public Rectangle SurfaceRectangle => new Rectangle(Devices.Select(x => x.Key.DeviceRectangle));

        #endregion

        #region Events

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <inheritdoc />
        public event ExceptionEventHandler Exception;

        /// <inheritdoc />
        public event UpdatingEventHandler Updating;

        /// <inheritdoc />
        public event UpdatedEventHandler Updated;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBSurface"/> class.
        /// </summary>
        public RGBSurface()
        {
            _lastUpdate = DateTime.Now;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Update(bool flushLeds = false)
        {
            OnUpdating();

            lock (_ledGroups)
            {
                // Update effects
                foreach (ILedGroup ledGroup in _ledGroups)
                    ledGroup.UpdateEffects();

                // Render brushes
                Render(this);
                foreach (ILedGroup ledGroup in _ledGroups.OrderBy(x => x.ZIndex))
                    Render(ledGroup);
            }

            foreach (IRGBDevice device in Devices.Keys)
                device.Update(flushLeds);

            OnUpdated();
        }

        /// <summary>
        /// Renders a ledgroup.
        /// </summary>
        /// <param name="ledGroup">The led group to render.</param>
        private void Render(ILedGroup ledGroup)
        {
            IList<Led> leds = ledGroup.GetLeds().ToList();
            IBrush brush = ledGroup.Brush;

            try
            {
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

                brush.UpdateEffects();
                brush.PerformFinalize();

                foreach (KeyValuePair<BrushRenderTarget, Color> renders in brush.RenderedTargets)
                    renders.Key.Led.Color = renders.Value;
            }
            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        private Rectangle GetDeviceLedLocation(Led led, Point extraOffset = null)
        {
            Point deviceLocation;
            if (!Devices.TryGetValue(led.Device, out deviceLocation))
                deviceLocation = new Point();

            return extraOffset != null
                       ? new Rectangle(led.LedRectangle.Location + deviceLocation + extraOffset, led.LedRectangle.Size)
                       : new Rectangle(led.LedRectangle.Location + deviceLocation, led.LedRectangle.Size);
        }

        /// <inheritdoc />
        public void PositionDevice(IRGBDevice device, Point location)
        {
            if (device == null) return;

            lock (Devices)
                Devices[device] = location ?? new Point();
        }

        /// <summary>
        /// Attaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachLedGroup(ILedGroup ledGroup)
        {
            if (ledGroup is IRGBSurface) return false;
            if (ledGroup == null) return false;

            lock (_ledGroups)
            {
                if (_ledGroups.Contains(ledGroup)) return false;

                _ledGroups.AddLast(ledGroup);
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
            if (ledGroup is IRGBSurface) return false;
            if (ledGroup == null) return false;

            lock (_ledGroups)
            {
                LinkedListNode<ILedGroup> node = _ledGroups.Find(ledGroup);
                if (node == null) return false;

                _ledGroups.Remove(node);
                return true;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Led> GetLeds()
        {
            return Devices.SelectMany(d => d.Key);
        }

        #region EventCaller

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        protected virtual void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(this, new ExceptionEventArgs(ex));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        protected virtual void OnUpdating()
        {
            try
            {
                long lastUpdateTicks = _lastUpdate.Ticks;
                _lastUpdate = DateTime.Now;
                Updating?.Invoke(this, new UpdatingEventArgs((DateTime.Now.Ticks - lastUpdateTicks) / 10000000.0));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        protected virtual void OnUpdated()
        {
            try
            {
                Updated?.Invoke(this, new UpdatedEventArgs());
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        #endregion

        #endregion
    }
}
