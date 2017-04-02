using System;
using System.Collections.Generic;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Contains all the hardware-id mappings for CoolerMaster devices.
    /// </summary>
    internal static class CoolerMasterKeyboardLedMappings
    {
        #region Properties & Fields

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysL_US = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysL_EU = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysM_US = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysM_EU = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysS_US = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        private static readonly Dictionary<CoolerMasterLedIds, Tuple<int, int>> MasterKeysS_EU = new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
        {
            { CoolerMasterLedIds.A, new Tuple<int, int>(3,1) },
            { CoolerMasterLedIds.S, new Tuple<int, int>(3,2) },
            { CoolerMasterLedIds.D, new Tuple<int, int>(3,3) },
            { CoolerMasterLedIds.F, new Tuple<int, int>(3,4) },
        };

        /// <summary>
        /// Contains all the hardware-id mappings for CoolerMaster devices.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Dictionary<CoolerMasterDevicesIndexes, Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>> Mapping =
            new Dictionary<CoolerMasterDevicesIndexes, Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>>
            {
                { CoolerMasterDevicesIndexes.MasterKeys_L, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysL_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysL_EU }
                  }
                },

                { CoolerMasterDevicesIndexes.MasterKeys_M, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysM_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysM_EU }
                  }
                },

                { CoolerMasterDevicesIndexes.MasterKeys_S, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysS_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysS_EU }
                  }
                },

                { CoolerMasterDevicesIndexes.MasterKeys_L_White, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysL_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysL_EU }
                  }
                },

                { CoolerMasterDevicesIndexes.MasterKeys_M_White, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysM_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysM_EU }
                  }
                },

                { CoolerMasterDevicesIndexes.MasterKeys_S_White, new Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
                  {
                    { CoolerMasterPhysicalKeyboardLayout.US, MasterKeysS_US },
                    { CoolerMasterPhysicalKeyboardLayout.EU, MasterKeysS_EU }
                  }
                },
            };

        #endregion
    }
}
