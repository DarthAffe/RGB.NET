// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Layout;

namespace RGB.NET.Devices.Debug;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for debug devices.
/// </summary>
public class DebugDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static DebugDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="DebugDeviceProvider"/> instance.
    /// </summary>
    public static DebugDeviceProvider Instance => _instance ?? new DebugDeviceProvider();

    private List<(IDeviceLayout layout, Action<IEnumerable<Led>>? updateLedsAction)> _fakeDeviceDefinitions = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DebugDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public DebugDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(DebugDeviceProvider)}");
        _instance = this;
    }

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