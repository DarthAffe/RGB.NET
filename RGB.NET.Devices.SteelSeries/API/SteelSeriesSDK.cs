using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using RGB.NET.Devices.SteelSeries.API.Model;

namespace RGB.NET.Devices.SteelSeries.API;

internal static class SteelSeriesSDK
{
    #region Constants

    private const string GAME_NAME = "RGBNET";
    private const string GAME_DISPLAYNAME = "RGB.NET";
    private const string EVENT_NAME = "UPDATELEDS";
    private static readonly string HANDLER = $@"(define (getZone x)
  (case x
    {string.Join(Environment.NewLine, Enum.GetValues(typeof(SteelSeriesLedId))
                                          .Cast<SteelSeriesLedId>()
                                          .Select(x => x.GetAPIName())
                                          .Select(ledId => $"    ((\"{ledId}\") {ledId}:)"))}
  ))

(handler ""{EVENT_NAME}""
  (lambda (data)
    (let* ((device (value: data))
           (zones (zones: data))
           (colors (colors: data)))
      (on-device device show-on-zones: colors (map (lambda (x) (getZone x)) zones)))))

(add-event-per-key-zone-use ""{EVENT_NAME}"" ""all"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-1-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-2-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-3-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-4-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-5-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-6-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-7-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-8-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-12-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-17-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-24-zone"")
(add-event-zone-use-with-specifier ""{EVENT_NAME}"" ""all"" ""rgb-103-zone"")

(add-custom-zone '(""num-5"" 93))"; //HACK DarthAffe 07.10.2021: Custom zone to workaround a SDK-issue (https://github.com/SteelSeries/gamesense-sdk/issues/85)

    private const string CORE_PROPS_WINDOWS = "%PROGRAMDATA%/SteelSeries/SteelSeries Engine 3/coreProps.json";
    private const string CORE_PROPS_OSX = "/Library/Application Support/SteelSeries Engine 3/coreProps.json";

    #endregion

    #region Properties & Fields
    // ReSharper disable InconsistentNaming

    private static readonly HttpClient _client = new();
    private static readonly Game _game = new(GAME_NAME, GAME_DISPLAYNAME);
    private static readonly Event _event = new(_game, EVENT_NAME);
    private static string? _baseUrl;

    internal static bool IsInitialized => !string.IsNullOrWhiteSpace(_baseUrl);

    // ReSharper restore InconsistentNaming
    #endregion

    #region Methods

    internal static bool Initialize()
    {
        try
        {
            string corePropsPath = GetCorePropsPath();
            if (!string.IsNullOrWhiteSpace(corePropsPath) && File.Exists(corePropsPath))
            {
                CoreProps? coreProps = JsonSerializer.Deserialize<CoreProps>(File.ReadAllText(corePropsPath));
                _baseUrl = coreProps?.Address;
                if (_baseUrl != null)
                {
                    if (!_baseUrl.StartsWith("http://", StringComparison.Ordinal))
                        _baseUrl = "http://" + _baseUrl;

                    RegisterGame(_game);
                    RegisterGoLispHandler(new GoLispHandler(_game, HANDLER));
                }
            }
        }
        catch
        {
            _baseUrl = null;
        }
        return IsInitialized;
    }

    internal static void UpdateLeds(string device, IList<(string zone, int[] color)> data)
    {
        _event.Data.Clear();
        _event.Data.Add("value", device);
        _event.Data.Add("colors", data.Select(x => x.color).ToList());
        _event.Data.Add("zones", data.Select(x => x.zone).ToList());

        TriggerEvent(_event);
    }

    internal static void SendHeartbeat() => SendHeartbeat(_game);

    internal static void ResetLeds() => StopGame(_game);

    internal static void Dispose()
    {
        if (IsInitialized)
            ResetLeds();

        _client.Dispose();
    }

#pragma warning disable IDE0051 // Remove unused private members
    // ReSharper disable UnusedMethodReturnValue.Local
    // ReSharper disable UnusedMember.Local
    private static string TriggerEvent(Event e) => PostJson("/game_event", e);
    private static string RegisterGoLispHandler(GoLispHandler handler) => PostJson("/load_golisp_handlers", handler);
    private static string RegisterEvent(Event e) => PostJson("/register_game_event", e);
    private static string UnregisterEvent(Event e) => PostJson("/remove_game_event", e);
    private static string RegisterGame(Game game) => PostJson("/game_metadata", game);
    private static string UnregisterGame(Game game) => PostJson("/remove_game", game);
    private static string StopGame(Game game) => PostJson("/stop_game", game);
    private static string SendHeartbeat(Game game) => PostJson("/game_heartbeat", game);
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedMethodReturnValue.Local
#pragma warning restore IDE0051 // Remove unused private members

    private static string PostJson(string urlSuffix, object o)
    {
        string payload = JsonSerializer.Serialize(o);
        return _client.PostAsync(_baseUrl + urlSuffix, new StringContent(payload, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
    }

    private static string GetCorePropsPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Environment.ExpandEnvironmentVariables(CORE_PROPS_WINDOWS);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return CORE_PROPS_OSX;

        throw new InvalidOperationException("Unknown operating system.");
    }

    #endregion
}