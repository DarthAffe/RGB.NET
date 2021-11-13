using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGB.NET.Core;

/// <summary>
/// Represents the abstract base implementation for a <see cref="IRGBDeviceProvider"/>.
/// </summary>
public abstract class AbstractRGBDeviceProvider : IRGBDeviceProvider
{
    #region Properties & Fields

    private readonly double _defaultUpdateRateHardLimit;

    /// <inheritdoc />
    public bool IsInitialized { get; protected set; }

    /// <inheritdoc />
    public bool ThrowsExceptions { get; protected set; }

    /// <inheritdoc />
    public virtual IEnumerable<IRGBDevice> Devices { get; protected set; } = Enumerable.Empty<IRGBDevice>();

    /// <summary>
    /// Gets the dictionary containing the registered update triggers.
    /// Normally <see cref="UpdateTriggers"/> should be used to access them.
    /// </summary>
    protected Dictionary<int, IDeviceUpdateTrigger> UpdateTriggerMapping { get; } = new();

    /// <inheritdoc />
    public IReadOnlyList<(int id, IDeviceUpdateTrigger trigger)> UpdateTriggers => new ReadOnlyCollection<(int id, IDeviceUpdateTrigger trigger)>(UpdateTriggerMapping.Select(x => (x.Key, x.Value)).ToList());

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler<ExceptionEventArgs>? Exception;

    #endregion

    #region Constructors
        
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRGBDeviceProvider" /> class.
    /// </summary>
    /// <param name="defaultUpdateRateHardLimit">The update rate hard limit all update triggers for this device provider are initialized with.</param>
    protected AbstractRGBDeviceProvider(double defaultUpdateRateHardLimit = 0)
    {
        this._defaultUpdateRateHardLimit = defaultUpdateRateHardLimit;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
    {
        ThrowsExceptions = throwExceptions;

        try
        {
            Reset();

            InitializeSDK();

            Devices = new ReadOnlyCollection<IRGBDevice>(GetLoadedDevices(loadFilter).ToList());

            foreach (IDeviceUpdateTrigger updateTrigger in UpdateTriggerMapping.Values)
                updateTrigger.Start();

            IsInitialized = true;
        }
        catch (DeviceProviderException)
        {
            Reset();
            throw;
        }
        catch (Exception ex)
        {
            Reset();
            Throw(ex, true);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Loads devices and returns a filtered list of them.
    /// </summary>
    /// <remarks> 
    /// The underlying loading of the devices happens in <see cref="LoadDevices"/>.
    /// </remarks>
    /// <param name="loadFilter"><see cref="RGBDeviceType"/>-flags to filter the device with.</param>
    /// <returns>The filtered collection of loaded devices.</returns>
    protected virtual IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
    {
        List<IRGBDevice> devices = new();
        foreach (IRGBDevice device in LoadDevices())
        {
            try
            {
                if (loadFilter.HasFlag(device.DeviceInfo.DeviceType))
                    devices.Add(device);
                else
                    device.Dispose();
            }
            catch (Exception ex)
            {
                Throw(ex);
            }
        }

        return devices;
    }

    /// <summary>
    /// Initializes the underlying SDK.
    /// </summary>
    protected abstract void InitializeSDK();

    /// <summary>
    /// Loads all devices this device provider is capable of loading.
    /// </summary>
    /// <remarks> 
    /// Filtering happens in <see cref="GetLoadedDevices"/>.
    /// </remarks>
    /// <returns>A collection of loaded devices.</returns>
    protected abstract IEnumerable<IRGBDevice> LoadDevices();

    /// <summary>
    /// Gets the <see cref="IDeviceUpdateTrigger"/> mapped to the specified id or a new one if the id wasn't requested before.
    /// </summary>
    /// <remarks>
    /// The creation of the update trigger happens in <see cref="CreateUpdateTrigger"/>.
    /// </remarks>
    /// <param name="id">The id of the update trigger.</param>
    /// <param name="updateRateHardLimit">The update rate hard limit to be set in the update trigger.</param>
    /// <returns>The update trigger mapped to the specified id.</returns>
    protected virtual IDeviceUpdateTrigger GetUpdateTrigger(int id = -1, double? updateRateHardLimit = null)
    {
        if (!UpdateTriggerMapping.TryGetValue(id, out IDeviceUpdateTrigger? updaeTrigger))
            UpdateTriggerMapping[id] = (updaeTrigger = CreateUpdateTrigger(id, updateRateHardLimit ?? _defaultUpdateRateHardLimit));

        return updaeTrigger;
    }

    /// <summary>
    /// Creates a update trigger with the specified id and the specified update rate hard limit.
    /// </summary>
    /// <param name="id">The id of the update trigger.</param>
    /// <param name="updateRateHardLimit">The update rate hard limit tobe  set in the update trigger.</param>
    /// <returns>The newly created update trigger.</returns>
    protected virtual IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit) => new DeviceUpdateTrigger(updateRateHardLimit);


    /// <summary>
    /// Resets the device provider and disposes all devices and update triggers.
    /// </summary>
    protected virtual void Reset()
    {
        foreach (IDeviceUpdateTrigger updateTrigger in UpdateTriggerMapping.Values)
            updateTrigger.Dispose();

        foreach (IRGBDevice device in Devices)
            device.Dispose();

        Devices = Enumerable.Empty<IRGBDevice>();
        UpdateTriggerMapping.Clear();
        IsInitialized = false;
    }

    /// <summary>
    /// Triggers the <see cref="Exception"/>-event and throws the specified exception if <see cref="ThrowsExceptions"/> is true and it is not overriden in the event.
    /// </summary>
    /// <param name="ex">The exception to throw.</param>
    /// <param name="isCritical">Indicates if the exception is critical for device provider to work correctly.</param>
    protected virtual void Throw(Exception ex, bool isCritical = false)
    {
        ExceptionEventArgs args = new(ex, isCritical, ThrowsExceptions);
        try { OnException(args); } catch { /* we don't want to throw due to bad event handlers */ }

        if (args.Throw)
            throw new DeviceProviderException(ex, isCritical);
    }

    /// <summary>
    /// Throws the <see cref="Exception"/> event.
    /// </summary>
    /// <param name="args">The parameters passed to the event.</param>
    protected virtual void OnException(ExceptionEventArgs args) => Exception?.Invoke(this, args);

    /// <inheritdoc />
    public virtual void Dispose()
    {
        Reset();

        GC.SuppressFinalize(this);
    }

    #endregion
}