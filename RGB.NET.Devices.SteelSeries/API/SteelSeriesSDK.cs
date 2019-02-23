using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using RGB.NET.Devices.SteelSeries.API.Model;

namespace RGB.NET.Devices.SteelSeries.API
{
    internal static class SteelSeriesSDK
    {
        #region Constants

        private const string GAME_NAME = "RGBNET";
        private const string GAME_DISPLAYNAME = "RGB.NET";
        private const string EVENT_NAME = "UPDATELEDS";
        private const string HANDLER = @"(handler """ + EVENT_NAME + @"""
  (lambda (data)
    (let* ((device (value: data))
           (zoneData (frame: data))
           (zones (frame-keys zoneData)))
      (do ((zoneDo zones (cdr zoneDo)))
          ((nil? zoneDo))
          (let* ((zone (car zoneDo))
                 (color (get-slot zoneData zone)))
            (on-device device show-on-zone: color zone))))))";

        private const string CORE_PROPS_WINDOWS = "%PROGRAMDATA%/SteelSeries/SteelSeries Engine 3/coreProps.json";
        private const string CORE_PROPS_OSX = "/Library/Application Support/SteelSeries Engine 3/coreProps.json";

        #endregion

        #region Properties & Fields
        // ReSharper disable InconsistentNaming

        private static readonly HttpClient _client = new HttpClient();
        private static readonly Game _game = new Game(GAME_NAME, GAME_DISPLAYNAME);
        private static readonly Event _event = new Event(_game, EVENT_NAME);
        private static string _baseUrl;

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
                    CoreProps coreProps = JsonConvert.DeserializeObject<CoreProps>(File.ReadAllText(corePropsPath));
                    _baseUrl = coreProps.Address;
                    if (!_baseUrl.StartsWith("http://", StringComparison.Ordinal))
                        _baseUrl = "http://" + _baseUrl;

                    RegisterGame(_game);
                    RegisterGoLispHandler(new GoLispHandler(_game, HANDLER));
                }
            }
            catch
            {
                _baseUrl = null;
            }
            return IsInitialized;
        }

        internal static void UpdateLeds(string device, Dictionary<string, int[]> data)
        {
            _event.Data.Clear();
            _event.Data.Add("value", device);
            _event.Data.Add("frame", data);

            TriggerEvent(_event);
        }

        internal static void SendHeartbeat() => SendHeartbeat(_game);

        internal static void ResetLeds() => StopGame(_game);

        internal static void Dispose()
        {
            ResetLeds();
            _client.Dispose();
        }

        private static string TriggerEvent(Event e) => PostJson("/game_event", e);
        private static string RegisterGoLispHandler(GoLispHandler handler) => PostJson("/load_golisp_handlers", handler);
        private static string RegisterEvent(Event e) => PostJson("/register_game_event", e);
        private static string UnregisterEvent(Event e) => PostJson("/remove_game_event", e);
        private static string RegisterGame(Game game) => PostJson("/game_metadata", game);
        private static string UnregisterGame(Game game) => PostJson("/remove_game", game);
        private static string StopGame(Game game) => PostJson("/stop_game", game);
        private static string SendHeartbeat(Game game) => PostJson("/game_heartbeat", game);

        private static string PostJson(string urlSuffix, object o)
        {
            string payload = JsonConvert.SerializeObject(o);
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
}
