using System.Collections.Generic;

namespace RGB.NET.Core;

public abstract class AbstractReferenceCounting : IReferenceCounting
{
    #region Properties & Fields

    private readonly HashSet<object> _referencingObjects = new();

    /// <inheritdoc />
    public int ActiveReferenceCount
    {
        get
        {
            lock (_referencingObjects)
                return _referencingObjects.Count;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void AddReferencingObject(object obj)
    {
        lock (_referencingObjects)
            if (!_referencingObjects.Contains(obj))
                _referencingObjects.Add(obj);
    }

    /// <inheritdoc />
    public void RemoveReferencingObject(object obj)
    {
        lock (_referencingObjects)
            _referencingObjects.Remove(obj);
    }

    #endregion
}