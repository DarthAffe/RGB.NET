using System;
using System.Collections.Generic;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Contains all the hardware-id mappings for CoolerMaster devices.
    /// </summary>
    internal static class CoolerMasterMouseLedMappings
    {
        #region Properties & Fields

        /// <summary>
        /// Contains all the hardware-id mappings for CoolerMaster devices.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Dictionary<CoolerMasterDevicesIndexes, Dictionary<CoolerMasterLedIds, Tuple<int, int>>> Mapping =
            new Dictionary<CoolerMasterDevicesIndexes, Dictionary<CoolerMasterLedIds, Tuple<int, int>>>
            {
                { CoolerMasterDevicesIndexes.MasterMouse_L, new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
                  {
                    { CoolerMasterLedIds.Side1, new Tuple<int, int>(0,0) },
                    { CoolerMasterLedIds.Side2, new Tuple<int, int>(1,0) },
                    { CoolerMasterLedIds.Side3, new Tuple<int, int>(2,0) },
                    { CoolerMasterLedIds.Back1, new Tuple<int, int>(3,0) },
                  }
                },

                { CoolerMasterDevicesIndexes.MasterMouse_S, new Dictionary<CoolerMasterLedIds, Tuple<int, int>>
                  {
                    { CoolerMasterLedIds.Back1, new Tuple<int, int>(0,0) },
                    { CoolerMasterLedIds.Wheel, new Tuple<int, int>(1,0) },
                    { CoolerMasterLedIds.Side3, new Tuple<int, int>(2,0) },
                    { CoolerMasterLedIds.Back1, new Tuple<int, int>(3,0) },
                  }
                },
            };

        #endregion
    }
}
