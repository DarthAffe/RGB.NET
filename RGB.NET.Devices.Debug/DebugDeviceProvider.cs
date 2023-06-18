// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Threading;
using RGB.NET.Core;
using RGB.NET.Layout;

namespace RGB.NET.Devices.Debug;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for debug devices.
/// </summary>
public sealed class DebugDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static Lazy<DebugDeviceProvider> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// Gets the singleton <see cref="DebugDeviceProvider"/> instance.
    /// </summary>
    public static DebugDeviceProvider Instance => _instance.Value;

    private List<(IDeviceLayout layout, Action<IEnumerable<Led>>? updateLedsAction)> _fakeDeviceDefinitions = new();

    #endregion

    #region Methods

    /// <summary>
    /// Adds a new fake device definition.
    /// </summary>
    /// <param name="layout">The path of the layout file to be used.</param>
    /// <param name="updateLedsAction">A action emulating led-updates.</param>
    public void AddFakeDeviceDefinition(IDeviceLayout layout, Action<IEnumerable<Led>>? updateLedsAction = null)
        => _fakeDeviceDefinitions.Add((layout, updateLedsAction));

    /// <summary>
    /// Removes all previously added fake device definitions.
    /// </summary>
    public void ClearFakeDeviceDefinitions() => _fakeDeviceDefinitions.Clear();

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach ((IDeviceLayout layout, Action<IEnumerable<Led>>? updateLedsAction) in _fakeDeviceDefinitions)
            yield return new DebugRGBDevice(layout, updateLedsAction);
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        _fakeDeviceDefinitions.Clear();
    }

    #endregion
}