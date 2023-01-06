using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <summary>
/// Represents custom LED data for ASUS keyboard LEDs.
/// </summary>
public record AsusKeyboardLedCustomData(AsusLedType LedType, int Id);

/// <summary>
/// Represents a record containing regex that matches to an ASUS device model and a LED mapping mapping to Light indexes.
/// </summary>
public record AsusKeyboardExtraMapping(Regex Regex, LedMapping<int> LedMapping);

/// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Asus keyboard.
/// </summary>
public class AsusKeyboardRGBDevice : AsusRGBDevice<AsusKeyboardRGBDeviceInfo>, IKeyboard
{
    #region Properties & Fields

    private readonly LedMapping<AsusLedId>? _ledMapping;
    private readonly Dictionary<LedId, AsusLedId> _ledAsusLed = new();
    private readonly Dictionary<LedId, int> _ledAsusLights = new();

    IKeyboardDeviceInfo IKeyboard.DeviceInfo => DeviceInfo;

    /// <summary>
    /// Gets or sets a list of extra LED mappings to apply to modes that match the provided regex.
    /// <para>Note: These LED mappings should be based on light indexes.</para>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static readonly List<AsusKeyboardExtraMapping> ExtraLedMappings = new()
                                                                             {
                                                                                 new AsusKeyboardExtraMapping(new Regex("(ROG Zephyrus Duo 15).*?"), LedMappings.ROGZephyrusDuo15)
                                                                             };

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by Asus for the keyboard.</param>
    /// <param name="ledMapping">A mapping of leds this device is initialized with.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal AsusKeyboardRGBDevice(AsusKeyboardRGBDeviceInfo info, LedMapping<AsusLedId>? ledMapping, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        this._ledMapping = ledMapping;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        if (DeviceInfo.Device.Type != (uint)AsusDeviceType.NB_KB_4ZONE_RGB)
        {
            int pos = 0;
            int unknownLed = (int)LedId.Unknown1;

            foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
            {
                if ((_ledMapping != null) && _ledMapping.TryGetValue((AsusLedId)key.Code, out LedId ledId))
                    AddAsusLed((AsusLedId)key.Code, ledId, new Point(pos++ * 19, 0), new Size(19, 19));
                else
                {
                    AddAsusLed((AsusLedId)key.Code, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                    unknownLed++;
                }
            }

            // Add extra LED mapping if required
            AsusKeyboardExtraMapping? extraMapping = ExtraLedMappings.FirstOrDefault(m => m.Regex.IsMatch(this.DeviceInfo.Model));
            if (extraMapping != null)
            {
                foreach ((LedId ledId, int lightIndex) in extraMapping.LedMapping)
                    AddAsusLed(lightIndex, ledId, new Point(pos++ * 19, 0), new Size(19, 19));
            }
        }
        else
        {
            int ledCount = DeviceInfo.Device.Lights.Count;
            for (int i = 0; i < ledCount; i++)
                AddLed(LedId.Keyboard_Custom1 + i, new Point(i * 19, 0), new Size(19, 19));
        }
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId)
    {
        if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId asusLedId))
            return new AsusKeyboardLedCustomData(AsusLedType.Key, (int)asusLedId);
        if (this._ledAsusLights.TryGetValue(ledId, out int lightIndex))
            return new AsusKeyboardLedCustomData(AsusLedType.Light, lightIndex);
        return null;
    }

    /// <summary>
    /// Add an ASUS LED by its LED ID
    /// </summary>
    private void AddAsusLed(AsusLedId asusLedId, LedId ledId, Point position, Size size)
    {
        if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId firstAsusLed))
            throw new RGBDeviceException($"Got LED '{ledId}' twice, first ASUS LED '{firstAsusLed}' "
                                       + $"second ASUS LED '{asusLedId}' on device '{DeviceInfo.DeviceName}'");

        this._ledAsusLed.Add(ledId, asusLedId);
        AddLed(ledId, position, size);
    }

    /// <summary>
    /// Add an ASUS LED by its light index
    /// </summary>
    private void AddAsusLed(int index, LedId ledId, Point position, Size size)
    {
        this._ledAsusLights.Add(ledId, index);
        AddLed(ledId, position, size);
    }

    #endregion
}