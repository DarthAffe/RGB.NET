namespace RGB.NET.Core;

public static class ReferenceCountingExtension
{
    public static bool HasActiveReferences(this IReferenceCounting target) => target.ActiveReferenceCount > 0;
}