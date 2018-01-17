// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Debug
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for debug devices.
    /// </summary>
    public class DebugDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static DebugDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="DebugDeviceProvider"/> instance.
        /// </summary>
        public static DebugDeviceProvider Instance => _instance ?? new DebugDeviceProvider();

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <inheritdoc />
        public bool HasExclusiveAccess { get; private set; }

        private List<(string layout, string imageLayout, Func<Dictionary<LedId, Color>> syncBackFunc, Action<IEnumerable<Led>> updateLedsAction)> _fakeDeviceDefinitions
            = new List<(string layout, string imageLayout, Func<Dictionary<LedId, Color>> syncBackFunc, Action<IEnumerable<Led>> updateLedsAction)>();

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
        /// <param name="imageLayout">The image-layout to load.</param>
        /// <param name="syncBackFunc">A function emulating device syncback.</param>
        /// <param name="updateLedsAction">A action emulating led-updates.</param>
        public void AddFakeDeviceDefinition(string layout, string imageLayout, Func<Dictionary<LedId, Color>> syncBackFunc = null, Action<IEnumerable<Led>> updateLedsAction = null)
            => _fakeDeviceDefinitions.Add((layout, imageLayout, syncBackFunc, updateLedsAction));

        /// <summary>
        /// Removes all previously added fake device definitions.
        /// </summary>
        public void ClearFakeDeviceDefinitions() => _fakeDeviceDefinitions.Clear();

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.Unknown, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;
            try
            {
                HasExclusiveAccess = exclusiveAccessIfPossible;
                IsInitialized = true;

                List<IRGBDevice> devices = new List<IRGBDevice>();
                foreach ((string layout, string imageLayout, Func<Dictionary<LedId, Color>> syncBackFunc, Action<IEnumerable<Led>> updateLedsAction) in _fakeDeviceDefinitions)
                {
                    DebugRGBDevice device = new DebugRGBDevice(layout, syncBackFunc, updateLedsAction);
                    device.Initialize(layout, imageLayout);
                    devices.Add(device);
                }

                Devices = devices;

                return true;
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }
        }

        /// <inheritdoc />
        public void ResetDevices()
        { }

        #endregion
    }
}
