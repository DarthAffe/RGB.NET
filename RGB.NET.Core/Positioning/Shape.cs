using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains a list of different shapes used to define the layout of a LED.
/// </summary>
[Serializable]
public enum Shape
{
    /// <summary>
    /// A custom shape defined by vector-data.
    /// </summary>
    Custom = 0,

    /// <summary>
    /// A simple rectangle.
    /// </summary>
    Rectangle = 1,

    /// <summary>
    /// A simple circle.
    /// </summary>
    Circle = 2,
}