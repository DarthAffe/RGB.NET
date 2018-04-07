using System.Collections.Generic;

namespace RGB.NET.Core
{
    public class CustomUpdateData
    {
        #region Properties & Fields

        private Dictionary<string, object> _data = new Dictionary<string, object>();

        #endregion

        #region Indexer

        public object this[string key]
        {
            get => _data.TryGetValue(key?.ToUpperInvariant(), out object data) ? data : default;
            set => _data[key?.ToUpperInvariant()] = value;
        }

        #endregion

        #region Constructors

        public CustomUpdateData()
        { }

        public CustomUpdateData(params (string key, object value)[] values)
        {
            foreach ((string key, object value) in values)
                this[key] = value;
        }

        #endregion
    }
}
