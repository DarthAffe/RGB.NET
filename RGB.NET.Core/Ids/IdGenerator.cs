﻿using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core;

/// <summary>
/// Offers some methods to create and handle unique identifiers.
/// </summary>
public static class IdGenerator
{
    #region Properties & Fields

    // ReSharper disable InconsistentNaming
    private static readonly HashSet<string> _registeredIds = [];
    private static readonly Dictionary<object, Dictionary<string, string>> _idMappings = [];
    private static readonly Dictionary<object, Dictionary<string, int>> _counter = [];
    // ReSharper restore InconsistentNaming

    #endregion

    #region Methods

    /// <summary>
    /// Makes the specified id unique based on the calling assembly by adding a counter if needed.
    /// </summary>
    /// <param name="id">The id to make unique.</param>
    /// <returns>The unique id.</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string MakeUnique(string id) => MakeUnique(Assembly.GetCallingAssembly(), id);

    public static string MakeUnique(object callingAssembly, string id)
    {
        if (!_idMappings.TryGetValue(callingAssembly, out Dictionary<string, string>? idMapping))
        {
            _idMappings.Add(callingAssembly, idMapping = []);
            _counter.Add(callingAssembly, []);
        }

        Dictionary<string, int> counterMapping = _counter[callingAssembly];

        if (!idMapping.TryGetValue(id, out string? mappedId))
        {
            mappedId = id;
            int mappingCounter = 1;
            while (_registeredIds.Contains(mappedId))
                mappedId = $"{id} ({++mappingCounter})";

            _registeredIds.Add(mappedId);
            idMapping.Add(id, mappedId);
        }

        counterMapping.TryAdd(mappedId, 0);

        int counter = ++counterMapping[mappedId];
        return counter <= 1 ? mappedId : $"{mappedId} ({counter})";
    }

    /// <summary>
    /// Resets the counter used to create unique ids.
    /// All previous generated ids are not garantueed to stay unique if this is called!
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ResetCounter() => ResetCounter(Assembly.GetCallingAssembly());

    public static void ResetCounter(object callingAssembly)
    {
        if (_counter.TryGetValue(callingAssembly, out Dictionary<string, int>? counter))
            counter.Clear();
    }

    #endregion
}