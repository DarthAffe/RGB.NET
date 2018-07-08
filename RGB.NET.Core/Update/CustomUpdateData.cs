using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a set of custom data, each indexed by a string-key.
    /// </summary>
    public class CustomUpdateData
    {
        #region Properties & Fields

        private Dictionary<string, object> _data = new Dictionary<string, object>();

        #endregion

        #region Indexer

        /// <summary>
        /// Gets or sets the value for a specific key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value represented by the given key.</returns>
        public object this[string key]
        {
            get => _data.TryGetValue(key?.ToUpperInvariant() ?? string.Empty, out object data) ? data : default;
            set => _data[key?.ToUpperInvariant() ?? string.Empty] = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomUpdateData"/> class.
        /// </summary>
        public CustomUpdateData()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomUpdateData"/> class.
        /// </summary>
        /// <param name="values">A params-list of tuples containing the key (string) and the value of a specific custom-data.</param>
        public CustomUpdateData(params (string key, object value)[] values)
        {
            foreach ((string key, object value) in values)
                this[key] = value;
        }

        #endregion
    }
}
