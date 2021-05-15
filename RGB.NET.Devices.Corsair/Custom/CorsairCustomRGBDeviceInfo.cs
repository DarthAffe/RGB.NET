// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairCustomRGBDevice" />.
    /// </summary>
    public class CorsairCustomRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        public CorsairLedId ReferenceCorsairLed { get; }
        public int LedCount { get; }
        internal int LedOffset { get; }

        #endregion

        #region Constructors

        //TODO DarthAffe 07.07.2018: DAP is a fan right now, that's most likely wrong
        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Corsair.CorsairCustomRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.Corsair._CorsairChannelDeviceInfo" />.</param>
        /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.Corsair.Native._CorsairDeviceInfo" />-struct</param>
        /// <param name="channelDeviceInfo">The native <see cref="T:RGB.NET.Devices.Corsair.Native._CorsairChannelDeviceInfo"/> representing this device.</param>
        /// <param name="referenceCorsairLed">The id of the first led of this device.</param>
        internal CorsairCustomRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo, _CorsairChannelDeviceInfo channelDeviceInfo, CorsairLedId referenceCorsairLed, int ledOffset)
            : base(deviceIndex, GetDeviceType(channelDeviceInfo.type), nativeInfo,
                   GetModelName(nativeInfo.model == IntPtr.Zero ? string.Empty : Regex.Replace(Marshal.PtrToStringAnsi(nativeInfo.model) ?? string.Empty, " ?DEMO", string.Empty, RegexOptions.IgnoreCase), channelDeviceInfo))
        {
            this.ReferenceCorsairLed = referenceCorsairLed;
            this.LedOffset = ledOffset;

            LedCount = channelDeviceInfo.deviceLedCount;
        }

        #endregion

        #region Methods

        private static RGBDeviceType GetDeviceType(CorsairChannelDeviceType deviceType)
        {
            switch (deviceType)
            {
                case CorsairChannelDeviceType.Invalid:
                    return RGBDeviceType.Unknown;

                case CorsairChannelDeviceType.FanHD:
                case CorsairChannelDeviceType.FanSP:
                case CorsairChannelDeviceType.FanLL:
                case CorsairChannelDeviceType.FanML:
                case CorsairChannelDeviceType.DAP:
                case CorsairChannelDeviceType.FanQL:
                case CorsairChannelDeviceType.FanSPPRO:
                    return RGBDeviceType.Fan;

                case CorsairChannelDeviceType.Strip:
                    return RGBDeviceType.LedStripe;

                case CorsairChannelDeviceType.Pump:
                case CorsairChannelDeviceType.WaterBlock:
                    return RGBDeviceType.Cooler;

                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null);
            }
        }

        private static string GetModelName(string model, _CorsairChannelDeviceInfo channelDeviceInfo)
        {
            switch (channelDeviceInfo.type)
            {
                case CorsairChannelDeviceType.Invalid:
                    return "Invalid";

                case CorsairChannelDeviceType.FanHD:
                    return "HD Fan";

                case CorsairChannelDeviceType.FanSP:
                    return "SP Fan";

                case CorsairChannelDeviceType.FanLL:
                    return "LL Fan";

                case CorsairChannelDeviceType.FanML:
                    return "ML Fan";

                case CorsairChannelDeviceType.FanQL:
                    return "QL Fan";

                case CorsairChannelDeviceType.FanSPPRO:
                    return "SP-PRO Fan";

                case CorsairChannelDeviceType.Strip:
                    // LS100 Led Strips are reported as one big strip if configured in monitor mode in iCUE, 138 LEDs for dual monitor, 84 for single
                    if ((model == "LS100 Starter Kit") && (channelDeviceInfo.deviceLedCount == 138))
                        return "LS100 LED Strip (dual monitor)";
                    else if ((model == "LS100 Starter Kit") && (channelDeviceInfo.deviceLedCount == 84))
                        return "LS100 LED Strip (single monitor)";
                    // Any other value means an "External LED Strip" in iCUE, these are reported per-strip, 15 for short strips, 27 for long
                    else if ((model == "LS100 Starter Kit") && (channelDeviceInfo.deviceLedCount == 15))
                        return "LS100 LED Strip (short)";
                    else if ((model == "LS100 Starter Kit") && (channelDeviceInfo.deviceLedCount == 27))
                        return "LS100 LED Strip (long)";
                    // Device model is "Commander Pro" for regular LED strips
                    else
                        return "LED Strip";

                case CorsairChannelDeviceType.DAP:
                    return "DAP Fan";

                case CorsairChannelDeviceType.WaterBlock:
                    return "Water Block";

                case CorsairChannelDeviceType.Pump:
                    return "Pump";

                default:
                    throw new ArgumentOutOfRangeException(nameof(channelDeviceInfo.type), channelDeviceInfo.type, null);
            }
        }

        #endregion
    }
}
