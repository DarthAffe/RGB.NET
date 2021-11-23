using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster;

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
    public static readonly Dictionary<CoolerMasterDevicesIndexes, Dictionary<LedId, (int row, int column)>> Mapping =
        new()
        {
            { CoolerMasterDevicesIndexes.MasterMouse_L, new Dictionary<LedId, (int row, int column)>
                                                        {
                                                            { LedId.Mouse1, (0,0) },
                                                            { LedId.Mouse2, (0,1) },
                                                            { LedId.Mouse3, (0,2) },
                                                            { LedId.Mouse4, (0,3) }
                                                        }
            },

            { CoolerMasterDevicesIndexes.MasterMouse_S, new Dictionary<LedId, (int row, int column)>
                                                        {
                                                            { LedId.Mouse1, (0,0) },
                                                            { LedId.Mouse2, (0,1) }
                                                        }
            },

            { CoolerMasterDevicesIndexes.MM530, new Dictionary<LedId, (int row, int column)>
                                                {
                                                    { LedId.Mouse1, (0,0) },
                                                    { LedId.Mouse2, (0,1) },
                                                    { LedId.Mouse3, (0,2) }
                                                }
            },

            { CoolerMasterDevicesIndexes.MM520, new Dictionary<LedId, (int row, int column)>
                                                {
                                                    { LedId.Mouse1, (0,0) },
                                                    { LedId.Mouse2, (0,1) },
                                                    { LedId.Mouse3, (0,2) }
                                                }
            },

            { CoolerMasterDevicesIndexes.MM830, new Dictionary<LedId, (int row, int column)>
                                                {
                                                    { LedId.Mouse1, (0,0) },
                                                    { LedId.Mouse2, (0,1) },
                                                    { LedId.Mouse3, (0,2) },
                                                    { LedId.Mouse4, (0,3) },
                                                    { LedId.Mouse5, (0,4) },
                                                    { LedId.Mouse6, (0,5) },
                                                    { LedId.Mouse7, (0,6) },
                                                    { LedId.Mouse8, (0,7) },
                                                    { LedId.Mouse9, (0,8) },
                                                    { LedId.Mouse10, (0,9) },
                                                }
            },
        };

    #endregion
}