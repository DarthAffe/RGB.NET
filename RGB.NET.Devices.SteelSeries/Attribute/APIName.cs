namespace RGB.NET.Devices.SteelSeries;

internal sealed class APIName(string name) : System.Attribute
{
    #region Properties & Fields

    public string Name { get; set; } = name;

    #endregion
}