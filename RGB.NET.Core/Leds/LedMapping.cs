using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core;

/// <summary>
/// Represents a mapping from <see cref="LedId"/> to a custom identifier.
/// </summary>
/// <typeparam name="T">The identifier the <see cref="LedId"/> is mapped to.</typeparam>
public class LedMapping<T> : IEnumerable<(LedId ledId, T mapping)>
    where T : notnull
{
    #region Properties & Fields

    private readonly Dictionary<LedId, T> _mapping = new();
    private readonly Dictionary<T, LedId> _reverseMapping = new();

    /// <summary>
    /// Gets the number of entries in this mapping.
    /// </summary>
    public int Count => _mapping.Count;

    /// <summary>
    /// Gets a collection of all mapped ledids.
    /// </summary>
    public ICollection<LedId> LedIds => _mapping.Keys;

    /// <summary>
    /// Gets a collection of all mapped custom identifiers.
    /// </summary>
    public ICollection<T> Mappings => _reverseMapping.Keys;

    #endregion

    #region Indexer

    /// <summary>
    /// Gets the custom identifier mapped to the specified <see cref="LedId"/>.
    /// </summary>
    /// <param name="ledId">The led id to get the mapped identifier.</param>
    /// <returns>The mapped ifentifier.</returns>
    public T this[LedId ledId]
    {
        get => _mapping[ledId];
        set
        {
            _mapping[ledId] = value;
            _reverseMapping[value] = ledId;
        }
    }

    /// <summary>
    /// Gets the <see cref="LedId"/> mapped to the specified custom identifier.
    /// </summary>
    /// <param name="mapping">The custom identifier to get the mapped led id.</param>
    /// <returns>The led id.</returns>
    public LedId this[T mapping]
    {
        get => _reverseMapping[mapping];
        set => this[value] = mapping;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a new entry to the mapping.
    /// </summary>
    /// <param name="ledId">The <see cref="LedId"/> to map.</param>
    /// <param name="mapping">The custom identifier to map.</param>
    public void Add(LedId ledId, T mapping)
    {
        _mapping.Add(ledId, mapping);
        _reverseMapping.Add(mapping, ledId);
    }

    /// <summary>
    /// Checks if the specified <see cref="LedId"/> is mapped.
    /// </summary>
    /// <param name="ledId">The led id to check.</param>
    /// <returns><c>true</c> if the led id is mapped; otherwise <c>false</c>.</returns>
    public bool Contains(LedId ledId) => _mapping.ContainsKey(ledId);

    /// <summary>
    /// Checks if the specified custom identifier is mapped.
    /// </summary>
    /// <param name="mapping">The custom identifier to check.</param>
    /// <returns><c>true</c> if the led id is mapped; otherwise <c>false</c>.</returns>
    public bool Contains(T mapping) => _reverseMapping.ContainsKey(mapping);

    /// <summary>
    /// Gets the custom identifier mapped to the specified led id.
    /// </summary>
    /// <param name="ledId">The led id to get the custom identifier  for.</param>
    /// <param name="mapping">Contains the mapped custom identifier or null if there is no mapping for the specified led id.</param>
    /// <returns><c>true</c> if there was a custom identifier  for the specified led id; otherwise <c>false</c>.</returns>
    public bool TryGetValue(LedId ledId, out T? mapping) => _mapping.TryGetValue(ledId, out mapping);

    /// <summary>
    /// Gets the led id mapped to the specified custom identifier.
    /// </summary>
    /// <param name="mapping">The custom identifier to get the led id for.</param>
    /// <param name="ledId">Contains the mapped led id or null if there is no mapping for the specified led id.</param>
    /// <returns><c>true</c> if there was a led id for the specified custom identifier; otherwise <c>false</c>.</returns>
    public bool TryGetValue(T mapping, out LedId ledId) => _reverseMapping.TryGetValue(mapping, out ledId);

    /// <summary>
    /// Removes the specified led id and the mapped custom identifier.
    /// </summary>
    /// <param name="ledId">The led id to remove.</param>
    /// <returns><c>true</c> if there was a mapping for the led id to remove; otherwise <c>false</c>.</returns>
    public bool Remove(LedId ledId)
    {
        if (_mapping.TryGetValue(ledId, out T? mapping))
            _reverseMapping.Remove(mapping);
        return _mapping.Remove(ledId);
    }

    /// <summary>
    /// Removes the specified custom identifier and the mapped led id.
    /// </summary>
    /// <param name="mapping">The custom identifier to remove.</param>
    /// <returns><c>true</c> if there was a mapping for the custom identifier to remove; otherwise <c>false</c>.</returns>
    public bool Remove(T mapping)
    {
        if (_reverseMapping.TryGetValue(mapping, out LedId ledId))
            _mapping.Remove(ledId);
        return _reverseMapping.Remove(mapping);
    }

    /// <summary>
    /// Removes all registered mappings.
    /// </summary>
    public void Clear()
    {
        _mapping.Clear();
        _reverseMapping.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<(LedId ledId, T mapping)> GetEnumerator() => _mapping.Select(x => (x.Key, x.Value)).GetEnumerator();

    #endregion
}