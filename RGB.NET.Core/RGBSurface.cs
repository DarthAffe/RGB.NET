// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private Stopwatch _deltaTimeCounter;

        private IList<IRGBDeviceProvider> _deviceProvider = new List<IRGBDeviceProvider>();
        private IList<IRGBDevice> _devices = new List<IRGBDevice>();
        private IList<IUpdateTrigger> _updateTriggers = new List<IUpdateTrigger>();

        // ReSharper disable InconsistentNaming

        private readonly LinkedList<ILedGroup> _ledGroups = new LinkedList<ILedGroup>();

        private readonly Rectangle _surfaceRectangle = new Rectangle();

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Gets a readonly list containing all loaded <see cref="IRGBDevice"/>.
        /// </summary>
        public IEnumerable<IRGBDevice> Devices => new ReadOnlyCollection<IRGBDevice>(_devices);

        /// <summary>
        /// Gets a readonly list containing all registered <see cref="IUpdateTrigger"/>.
        /// </summary>
        public IEnumerable<IUpdateTrigger> UpdateTriggers => new ReadOnlyCollection<IUpdateTrigger>(_updateTriggers);

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
            _deltaTimeCounter = Stopwatch.StartNew();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform a full update for all devices. Updates only dirty <see cref="Led"/> by default, or all <see cref="Led"/>, if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all <see cref="Led"/>, (including clean ones) should be updated.</param>
        public void Update(bool flushLeds = false) => Update(null, new CustomUpdateData(("flushLeds", flushLeds)));

        private void Update(object updateTrigger, CustomUpdateData customData) => Update(updateTrigger as IUpdateTrigger, customData);

        private void Update(IUpdateTrigger updateTrigger, CustomUpdateData customData)
        {
            if (customData == null)
                customData = new CustomUpdateData();

            try
            {
                bool flushLeds = customData["flushLeds"] as bool? ?? false;
                bool syncBack = customData["syncBack"] as bool? ?? true;
                bool render = customData["render"] as bool? ?? true;
                bool updateDevices = customData["updateDevices"] as bool? ?? true;

                lock (_updateTriggers)
                {
                    OnUpdating(updateTrigger, customData);

                    if (syncBack)
                        foreach (IRGBDevice device in Devices)
                            if (device.UpdateMode.HasFlag(DeviceUpdateMode.SyncBack) && device.DeviceInfo.SupportsSyncBack)
                                try { device.SyncBack(); }
                                catch (Exception ex) { OnException(ex); }

                    if (render)
                        lock (_ledGroups)
                        {
                            // Render brushes
                            foreach (ILedGroup ledGroup in _ledGroups.OrderBy(x => x.ZIndex))
                                try { Render(ledGroup); }
                                catch (Exception ex) { OnException(ex); }
                        }

                    if (updateDevices)
                        foreach (IRGBDevice device in Devices)
                            if (!device.UpdateMode.HasFlag(DeviceUpdateMode.NoUpdate))
                                try { device.Update(flushLeds); }
                                catch (Exception ex) { OnException(ex); }

                    OnUpdated();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            //if (_updateTokenSource?.IsCancellationRequested == false)
            //    _updateTokenSource.Cancel();

            foreach (IRGBDevice device in _devices)
                try { device.Dispose(); }
                catch { /* We do what we can */ }

            foreach (IRGBDeviceProvider deviceProvider in _deviceProvider)
                try { deviceProvider.Dispose(); }
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
                    Rectangle brushRectangle = new Rectangle(leds.Select(led => led.AbsoluteLedRectangle));
                    Point offset = new Point(-brushRectangle.Location.X, -brushRectangle.Location.Y);
                    brushRectangle.Location = new Point(0, 0);
                    brush.PerformRender(brushRectangle,
                                        leds.Select(x => new BrushRenderTarget(x, GetDeviceLedLocation(x, offset))));
                    break;
                case BrushCalculationMode.Absolute:
                    brush.PerformRender(SurfaceRectangle, leds.Select(x => new BrushRenderTarget(x, x.AbsoluteLedRectangle)));
                    break;
                default:
                    throw new ArgumentException();
            }

            //brush.UpdateEffects();
            brush.PerformFinalize();

            foreach (KeyValuePair<BrushRenderTarget, Color> renders in brush.RenderedTargets)
                renders.Key.Led.Color = renders.Value;
        }

        private Rectangle GetDeviceLedLocation(Led led, Point extraOffset)
        {
            Rectangle absoluteRectangle = led.AbsoluteLedRectangle;
            return (absoluteRectangle.Location + extraOffset) + absoluteRectangle.Size;
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

            _surfaceRectangle.Width = devicesRectangle.Location.X + devicesRectangle.Size.Width;
            _surfaceRectangle.Height = devicesRectangle.Location.Y + devicesRectangle.Size.Height;
        }

        /// <summary>
        /// Gets all devices of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of devices to get.</typeparam>
        /// <returns>A list of devices with the specified type.</returns>
        public IList<T> GetDevices<T>()
            where T : class
            => new ReadOnlyCollection<T>(_devices.Select(x => x as T).Where(x => x != null).ToList());

        /// <summary>
        /// Gets all devices of the specified <see cref="RGBDeviceType"/>.
        /// </summary>
        /// <param name="deviceType">The <see cref="RGBDeviceType"/> of the devices to get.</param>
        /// <returns>a list of devices matching the specified <see cref="RGBDeviceType"/>.</returns>
        public IList<IRGBDevice> GetDevices(RGBDeviceType deviceType)
            => new ReadOnlyCollection<IRGBDevice>(_devices.Where(x => x.DeviceInfo.DeviceType == deviceType).ToList());

        /// <summary>
        /// Registers the provided <see cref="IUpdateTrigger"/>.
        /// </summary>
        /// <param name="updateTrigger">The <see cref="IUpdateTrigger"/> to register.</param>
        public void RegisterUpdateTrigger(IUpdateTrigger updateTrigger)
        {
            if (!_updateTriggers.Contains(updateTrigger))
            {
                _updateTriggers.Add(updateTrigger);
                updateTrigger.Update += Update;
            }
        }

        /// <summary>
        /// Unregisters the provided <see cref="IUpdateTrigger"/>.
        /// </summary>
        /// <param name="updateTrigger">The <see cref="IUpdateTrigger"/> to unregister.</param>
        public void UnregisterUpdateTrigger(IUpdateTrigger updateTrigger)
        {
            if (_updateTriggers.Remove(updateTrigger))
                updateTrigger.Update -= Update;
        }

        #endregion
    }
}
