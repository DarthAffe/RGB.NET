// ReSharper disable EventNeverSubscribedTo.Global

using System;
using System.Collections.Generic;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic device provider.
/// </summary>
public interface IRGBDeviceProvider : IDisposable
{
    #region Properties & Fields

    /// <summary>
    /// Indicates if the used SDK is initialized and ready to use.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Indicates if exceptions in the device provider are thrown or silently ignored.
    /// </summary>
    /// <remarks>
    /// This should only be set to <c>true</c> for debugging/development purposes.
    /// Production code should use the <see cref="Exception"/>-Event to handle exceptions.
    /// </remarks>
    bool ThrowsExceptions { get; }

    /// <summary>
    /// Gets a collection of <see cref="IRGBDevice"/> loaded by this <see cref="IRGBDeviceProvider"/>.
    /// </summary>
    IReadOnlyList<IRGBDevice> Devices { get; }

    /// <summary>
    /// Gets a collection <see cref="IDeviceUpdateTrigger"/> registered to this device provider.
    /// </summary>
    IReadOnlyList<(int id, IDeviceUpdateTrigger trigger)> UpdateTriggers { get; }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when an exception is thrown in the device provider.
    /// </summary>
    event EventHandler<ExceptionEventArgs>? Exception;

    /// <summary>
    /// Occurs when device is added.
    /// </summary>
    event EventHandler<RGBDeviceAddedEventArgs>? DeviceAdded; 

    /// <summary>
    /// Occurs when device is removed.
    /// </summary>
    event EventHandler<RGBDeviceRemovedEventArgs>? DeviceRemoved; 

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the device provider and loads available devices.
    /// </summary>
    /// <param name="loadFilter"><see cref="RGBDeviceType"/>-flags to filter the devices to load.</param>
    /// <param name="throwExceptions">Specifies if exceptions should be thrown or silently be ignored.</param>
    /// <returns><c>true</c> if the initialization was successful; <c>false</c> otherwise.</returns>
    bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false);

    #endregion
}