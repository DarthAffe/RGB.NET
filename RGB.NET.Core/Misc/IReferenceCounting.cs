namespace RGB.NET.Core;

public interface IReferenceCounting
{
    /// <summary>
    /// Gets the amount of currently registered referencing objects.
    /// </summary>
    public int ActiveReferenceCount { get; }

    /// <summary>
    /// Adds the given object to the list of referencing objects.
    /// </summary>
    /// <param name="obj">The object to add.</param>
    public void AddReferencingObject(object obj);

    /// <summary>
    /// Removes the given object from the list of referencing objects.
    /// </summary>
    /// <param name="obj">The object to remove.</param>
    public void RemoveReferencingObject(object obj);
}