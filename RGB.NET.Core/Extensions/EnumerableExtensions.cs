using System.Collections.Generic;

namespace RGB.NET.Core;

public static class EnumerableExtensions
{
    public static List<T> ToList<T>(this IEnumerable<T> source, int capacity) 
    {
        List<T> res = new List<T>(capacity);
        res.AddRange(source);
        return res;
    }
}
