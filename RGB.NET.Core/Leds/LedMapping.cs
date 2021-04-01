using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    public class LedMapping<T> : IEnumerable<(LedId ledId, T mapping)>
        where T : notnull
    {
        #region Properties & Fields

        private readonly Dictionary<LedId, T> _mapping = new();
        private readonly Dictionary<T, LedId> _reverseMapping = new();

        public int Count => _mapping.Count;

        public ICollection<LedId> LedIds => _mapping.Keys;
        public ICollection<T> Mappings => _reverseMapping.Keys;

        #endregion

        #region Indexer

        public T this[LedId ledId]
        {
            get => _mapping[ledId];
            set
            {
                _mapping[ledId] = value;
                _reverseMapping[value] = ledId;
            }
        }

        public LedId this[T mapping]
        {
            get => _reverseMapping[mapping];
            set => this[value] = mapping;
        }

        #endregion

        #region Methods

        public void Add(LedId ledId, T mapping)
        {
            _mapping.Add(ledId, mapping);
            _reverseMapping.Add(mapping, ledId);
        }

        public bool Contains(LedId ledId) => _mapping.ContainsKey(ledId);
        public bool Contains(T mapping) => _reverseMapping.ContainsKey(mapping);

        public bool TryGetValue(LedId ledId, out T? mapping) => _mapping.TryGetValue(ledId, out mapping);
        public bool TryGetValue(T mapping, out LedId ledId) => _reverseMapping.TryGetValue(mapping, out ledId);

        public bool Remove(LedId ledId)
        {
            if (_mapping.TryGetValue(ledId, out T? mapping))
                _reverseMapping.Remove(mapping);
            return _mapping.Remove(ledId);
        }

        public bool Remove(T mapping)
        {
            if (_reverseMapping.TryGetValue(mapping, out LedId ledId))
                _mapping.Remove(ledId);
            return _reverseMapping.Remove(mapping);
        }

        public void Clear()
        {
            _mapping.Clear();
            _reverseMapping.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<(LedId ledId, T mapping)> GetEnumerator() => _mapping.Select(x => (x.Key, x.Value)).GetEnumerator();

        #endregion
    }
}
