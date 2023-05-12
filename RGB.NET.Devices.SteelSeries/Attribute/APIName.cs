namespace RGB.NET.Devices.SteelSeries;

internal sealed class APIName : System.Attribute
{
    #region Properties & Fields

    public string Name { get; set; }

    #endregion

    #region Constructors

    public APIName(string name)
    {
        this.Name = name;
    }

    #endregion
}