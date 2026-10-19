using System.Collections.Generic;
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
    // Keyed by the assembly's full name rather than the Assembly instance, so a reloaded copy of an assembly
    // (e.g. a plugin in a new AssemblyLoadContext) keeps its ids and unloaded assemblies aren't kept alive.
    private static readonly Dictionary<string, Dictionary<string, string>> _idMappings = [];
    private static readonly Dictionary<string, Dictionary<string, int>> _counter = [];
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

    internal static string MakeUnique(Assembly callingAssembly, string id)
    {
        string assemblyKey = GetAssemblyKey(callingAssembly);
        if (!_idMappings.TryGetValue(assemblyKey, out Dictionary<string, string>? idMapping))
        {
            _idMappings.Add(assemblyKey, idMapping = []);
            _counter.Add(assemblyKey, []);
        }

        Dictionary<string, int> counterMapping = _counter[assemblyKey];

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

    internal static void ResetCounter(Assembly callingAssembly)
    {
        if (_counter.TryGetValue(GetAssemblyKey(callingAssembly), out Dictionary<string, int>? counter))
            counter.Clear();
    }

    private static string GetAssemblyKey(Assembly assembly) => assembly.FullName ?? assembly.GetName().Name ?? string.Empty;

    #endregion
}