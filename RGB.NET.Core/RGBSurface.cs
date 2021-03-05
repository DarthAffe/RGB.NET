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
    public sealed class RGBSurface : AbstractBindable, IDisposable
    {
        #region Properties & Fields

        private Stopwatch _deltaTimeCounter;

        private readonly IList<IRGBDevice> _devices = new List<IRGBDevice>();
        private readonly IList<IUpdateTrigger> _updateTriggers = new List<IUpdateTrigger>();
        private readonly List<ILedGroup> _ledGroups = new List<ILedGroup>();

        /// <summary>
        /// Gets a readonly list containing all loaded <see cref="IRGBDevice"/>.
        /// </summary>
        public IEnumerable<IRGBDevice> Devices
        {
            get
            {
                lock (_devices)
                    return new ReadOnlyCollection<IRGBDevice>(_devices);
            }
        }

        /// <summary>
        /// Gets a readonly list containing all registered <see cref="IUpdateTrigger"/>.
        /// </summary>
        public IEnumerable<IUpdateTrigger> UpdateTriggers => new ReadOnlyCollection<IUpdateTrigger>(_updateTriggers);

        /// <summary>
        /// Gets a copy of the <see cref="Rectangle"/> representing this <see cref="RGBSurface"/>.
        /// </summary>
        public Rectangle Boundary { get; private set; } = new(new Point(0, 0), new Size(0, 0));

        /// <summary>
        /// Gets a list of all <see cref="Led"/> on this <see cref="RGBSurface"/>.
        /// </summary>
        public IEnumerable<Led> Leds
        {
            get
            {
                lock (_devices)
                    return _devices.SelectMany(x => x);
            }
        }

        #endregion

        #region EventHandler

        /// <summary>
        /// Represents the event-handler of the <see cref="Exception"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void ExceptionEventHandler(ExceptionEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="Updating"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void UpdatingEventHandler(UpdatingEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="Updated"/>-event.
        /// </summary>
        /// <param name="args">The arguments provided by the event.</param>
        public delegate void UpdatedEventHandler(UpdatedEventArgs args);

        /// <summary>
        /// Represents the event-handler of the <see cref="SurfaceLayoutChanged"/>-event.
        /// </summary>
        /// <param name="args"></param>
        public delegate void SurfaceLayoutChangedEventHandler(SurfaceLayoutChangedEventArgs args);

        #endregion

        #region Events

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <summary>
        /// Occurs when a catched exception is thrown inside the <see cref="RGBSurface"/>.
        /// </summary>
        public event ExceptionEventHandler? Exception;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> starts updating.
        /// </summary>
        public event UpdatingEventHandler? Updating;

        /// <summary>
        /// Occurs when the <see cref="RGBSurface"/> update is done.
        /// </summary>
        public event UpdatedEventHandler? Updated;

        /// <summary>
        /// Occurs when the layout of this <see cref="RGBSurface"/> changed.
        /// </summary>
        public event SurfaceLayoutChangedEventHandler? SurfaceLayoutChanged;

        // ReSharper restore EventNeverSubscribedTo.Global

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBSurface"/> class.
        /// </summary>
        public RGBSurface()
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

        private void Update(object? updateTrigger, CustomUpdateData customData) => Update(updateTrigger as IUpdateTrigger, customData);

        private void Update(IUpdateTrigger? updateTrigger, CustomUpdateData customData)
        {
            try
            {
                bool flushLeds = customData["flushLeds"] as bool? ?? false;
                bool render = customData["render"] as bool? ?? true;
                bool updateDevices = customData["updateDevices"] as bool? ?? true;

                lock (_updateTriggers)
                    lock (_devices)
                    {
                        OnUpdating(updateTrigger, customData);

                        if (render)
                            lock (_ledGroups)
                            {
                                // Render brushes
                                foreach (ILedGroup ledGroup in _ledGroups)
                                    try { Render(ledGroup); }
                                    catch (Exception ex) { OnException(ex); }
                            }

                        if (updateDevices)
                            foreach (IRGBDevice device in _devices)
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
            List<IRGBDevice> devices;
            lock (_devices)
                devices = new List<IRGBDevice>(_devices);

            foreach (IRGBDevice device in devices)
                try { Detach(device); }
                catch { /* We do what we can */}

            foreach (IUpdateTrigger updateTrigger in _updateTriggers)
                try { updateTrigger.Dispose(); }
                catch { /* We do what we can */}

            _ledGroups.Clear();
        }

        /// <summary>
        /// Renders a ledgroup.
        /// </summary>
        /// <param name="ledGroup">The led group to render.</param>
        /// <exception cref="ArgumentException">Thrown if the <see cref="IBrush.CalculationMode"/> of the Brush is not valid.</exception>
        private void Render(ILedGroup ledGroup)
        {
            IList<Led> leds = ledGroup.ToList();
            IBrush? brush = ledGroup.Brush;

            if ((brush == null) || !brush.IsEnabled) return;

            IEnumerable<(RenderTarget renderTarget, Color color)> render;
            switch (brush.CalculationMode)
            {
                case RenderMode.Relative:
                    Rectangle brushRectangle = new(leds.Select(led => led.AbsoluteBoundary));
                    Point offset = new(-brushRectangle.Location.X, -brushRectangle.Location.Y);
                    brushRectangle = brushRectangle.SetLocation(new Point(0, 0));
                    render = brush.Render(brushRectangle, leds.Select(led => new RenderTarget(led, led.AbsoluteBoundary.Translate(offset))));
                    break;
                case RenderMode.Absolute:
                    render = brush.Render(Boundary, leds.Select(led => new RenderTarget(led, led.AbsoluteBoundary)));
                    break;
                default:
                    throw new ArgumentException($"The CalculationMode '{brush.CalculationMode}' is not valid.");
            }

            foreach ((RenderTarget renderTarget, Color c) in render)
                renderTarget.Led.Color = c;
        }

        /// <summary>
        /// Attaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        public bool Attach(ILedGroup ledGroup)
        {
            lock (_ledGroups)
            {
                if (ledGroup.Surface != null) return false;

                ledGroup.Surface = this;
                _ledGroups.Add(ledGroup);
                _ledGroups.Sort((group1, group2) => group1.ZIndex.CompareTo(group2.ZIndex));
                ledGroup.OnAttach();

                return true;
            }
        }

        /// <summary>
        /// Detaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to detached.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be detached; otherwise, <c>false</c>.</returns>
        public bool Detach(ILedGroup ledGroup)
        {
            lock (_ledGroups)
            {
                if (!_ledGroups.Remove(ledGroup)) return false;
                ledGroup.OnDetach();
                ledGroup.Surface = null;

                return true;
            }
        }

        public void Attach(IRGBDevice device)
        {
            lock (_devices)
            {
                if (device.Surface != null) throw new RGBSurfaceException($"The device '{device.DeviceInfo.Manufacturer} {device.DeviceInfo.Model}' is already attached to a surface.");

                device.Surface = this;
                device.BoundaryChanged += DeviceOnBoundaryChanged;

                _devices.Add(device);
                OnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs.FromAddedDevice(device));
            }
        }

        public void Detach(IRGBDevice device)
        {
            lock (_devices)
            {
                if (!_devices.Contains(device)) throw new RGBSurfaceException($"The device '{device.DeviceInfo.Manufacturer} {device.DeviceInfo.Model}' is not attached to this surface.");

                device.BoundaryChanged -= DeviceOnBoundaryChanged;
                device.Surface = null;

                _devices.Remove(device);

                OnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs.FromRemovedDevice(device));
            }
        }

        // ReSharper restore UnusedMember.Global

        private void DeviceOnBoundaryChanged(object? sender, EventArgs args)
            => OnSurfaceLayoutChanged((sender is IRGBDevice device) ? SurfaceLayoutChangedEventArgs.FromChangedDevice(device) : SurfaceLayoutChangedEventArgs.Misc());

        private void OnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs args)
        {
            UpdateSurfaceRectangle();

            SurfaceLayoutChanged?.Invoke(args);
        }

        private void UpdateSurfaceRectangle()
        {
            lock (_devices)
            {
                Rectangle devicesRectangle = new(_devices.Select(d => d.Boundary));
                Boundary = Boundary.SetSize(new Size(devicesRectangle.Location.X + devicesRectangle.Size.Width, devicesRectangle.Location.Y + devicesRectangle.Size.Height));
            }
        }

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

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        private void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(new ExceptionEventArgs(ex));
            }
            catch { /* Well ... that's not my fault */ }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        private void OnUpdating(IUpdateTrigger? trigger, CustomUpdateData customData)
        {
            try
            {
                double deltaTime = _deltaTimeCounter.Elapsed.TotalSeconds;
                _deltaTimeCounter.Restart();
                Updating?.Invoke(new UpdatingEventArgs(deltaTime, trigger, customData));
            }
            catch { /* Well ... that's not my fault */ }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        private void OnUpdated()
        {
            try
            {
                Updated?.Invoke(new UpdatedEventArgs());
            }
            catch { /* Well ... that's not my fault */ }
        }

        #endregion
    }
}
