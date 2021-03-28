using System.Collections.Generic;

namespace RGB.NET.Core
{
    public static class DeviceHelper
    {
        #region Properties & Fields

        private static readonly Dictionary<string, int> MODEL_COUNTER = new();

        #endregion

        #region Methods

        public static string CreateDeviceName(string manufacturer, string model) => $"{manufacturer} {MakeUnique(model)}";

        public static string MakeUnique(string model)
        {
            if (MODEL_COUNTER.TryGetValue(model, out int _))
            {
                int counter = ++MODEL_COUNTER[model];
                return $"{model} {counter}";
            }
            else
            {
                MODEL_COUNTER.Add(model, 1);
                return model;
            }
        }

        #endregion
    }
}
