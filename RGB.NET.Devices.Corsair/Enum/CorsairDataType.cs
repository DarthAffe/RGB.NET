namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: contains list of available property types
/// </summary>
public enum CorsairDataType
{
    /// <summary>
    /// iCUE-SDK: for property of type Boolean
    /// </summary>
    Boolean = 0,

    /// <summary>
    /// iCUE-SDK: for property of type Int32 or Enumeration
    /// </summary>
    Int32 = 1,

    /// <summary>
    /// iCUE-SDK: for property of type Float64
    /// </summary>
    Float64 = 2,

    /// <summary>
    /// iCUE-SDK: for property of type String
    /// </summary>
    String = 3,

    /// <summary>
    /// iCUE-SDK: for array of Boolean
    /// </summary>
    BooleanArray = 16,

    /// <summary>
    /// iCUE-SDK: for array of Int32
    /// </summary>
    Int32Array = 17,

    /// <summary>
    /// iCUE-SDK: for array of Float64
    /// </summary>
    Float64Array = 18,

    /// <summary>
    /// iCUE-SDK: for array of String
    /// </summary>
    StringArray = 19
}