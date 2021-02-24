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
    public class RGBSurface : AbstractBindable, IDisposable
    {
        #region Properties & Fields

        private Stopwatch _deltaTimeCounter;

        private IList<IRGBDevice> _devices = new List<IRGBDevice>();
        private IList<IUpdateTrigger> _updateTriggers = new List<IUpdateTrigger>();

        // ReSharper disable InconsistentNaming

        private readonly LinkedList<ILedGroup> _ledGroups = new();

        // ReSharper restore InconsistentNaming

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
                                foreach (ILedGroup ledGroup in _ledGroups.OrderBy(x => x.ZIndex))
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
            IList<Led> leds = ledGroup.GetLeds().ToList();
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
            {
                Color color = c;
                for (int i = 0; i < renderTarget.Led.Device.ColorCorrections.Count; i++)
                    renderTarget.Led.Device.ColorCorrections[i].ApplyTo(ref color);

                renderTarget.Led.Color = color;
            }
        }

        /// <summary>
        /// Attaches the given <see cref="ILedGroup"/>.
        /// </summary>
        /// <param name="ledGroup">The <see cref="ILedGroup"/> to attach.</param>
        /// <returns><c>true</c> if the <see cref="ILedGroup"/> could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachLedGroup(ILedGroup ledGroup)
        {
            lock (_ledGroups)
            {
                if (_ledGroups.Contains(ledGroup)) return false;

                _ledGroups.AddLast(ledGroup);
                ledGroup.OnAttach(this);

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
            lock (_ledGroups)
            {
                LinkedListNode<ILedGroup>? node = _ledGroups.Find(ledGroup);
                if (node == null) return false;

                _ledGroups.Remove(node);
                node.Value.OnDetach(this);

                return true;
            }
        }

        public void Attach(IEnumerable<IRGBDevice> devices)
        {
            lock (_devices)
            {
                foreach (IRGBDevice device in devices)
                    Attach(device);
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

        public void Detach(IEnumerable<IRGBDevice> devices)
        {
            lock (_devices)
            {
                foreach (IRGBDevice device in devices)
                    Detach(device);
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

        /// <summary>
        /// Automatically aligns all devices to prevent overlaps.
        /// </summary>
        public void AlignDevices()
        {
            float posX = 0;
            foreach (IRGBDevice device in Devices)
            {
                device.Location += new Point(posX, 0);
                posX += device.ActualSize.Width + 1;
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
        /// Gets all devices of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of devices to get.</typeparam>
        /// <returns>A list of devices with the specified type.</returns>
        public IList<T> GetDevices<T>()
            where T : class
        {
            lock (_devices)
                return new ReadOnlyCollection<T>(_devices.Where(x => x is T).Cast<T>().ToList());
        }

        /// <summary>
        /// Gets all devices of the specified <see cref="RGBDeviceType"/>.
        /// </summary>
        /// <param name="deviceType">The <see cref="RGBDeviceType"/> of the devices to get.</param>
        /// <returns>a list of devices matching the specified <see cref="RGBDeviceType"/>.</returns>
        public IList<IRGBDevice> GetDevices(RGBDeviceType deviceType)
        {
            lock (_devices)
                return new ReadOnlyCollection<IRGBDevice>(_devices.Where(d => deviceType.HasFlag(d.DeviceInfo.DeviceType)).ToList());
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
