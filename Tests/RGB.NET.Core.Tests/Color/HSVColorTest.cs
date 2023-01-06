using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Color;

[TestClass]
public class HSVColorTest
{
    #region Manipulation
        
    #region Add

    [TestMethod]
    public void AddHueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(hue: 30);
            
        Assert.AreEqual(HSVColor.Create(210, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void AddHueWrapTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(hue: 220);

        Assert.AreEqual(HSVColor.Create(40, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void AddSaturationTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(saturation: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.8f, 0.5f), result);
    }

    [TestMethod]
    public void AddSaturationClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(saturation: 0.8f);

        Assert.AreEqual(HSVColor.Create(180, 1.0f, 0.5f), result);
    }

    [TestMethod]
    public void AddValueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(value: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 0.8f), result);
    }

    [TestMethod]
    public void AddValueClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.AddHSV(value: 0.8f);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 1.0f), result);
    }

    #endregion

    #region Subtract

    [TestMethod]
    public void SubtractHueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(hue: 30);

        Assert.AreEqual(HSVColor.Create(150, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractHueWrapTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(hue: 220);

        Assert.AreEqual(HSVColor.Create(320, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractSaturationTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(saturation: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.2f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractSaturationClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(saturation: 0.8f);

        Assert.AreEqual(HSVColor.Create(180, 0, 0.5f), result);
    }

    [TestMethod]
    public void SubtractValueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(value: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 0.2f), result);
    }

    [TestMethod]
    public void SubtractValueClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractHSV(value: 0.8f);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 0), result);
    }

    #endregion

    #region Multiply

    [TestMethod]
    public void MultiplyHueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.MultiplyHSV(hue: 1.5f);

        Assert.AreEqual(HSVColor.Create(270, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void MultiplyHueWrapTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.MultiplyHSV(hue: 3);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void MultiplySaturationTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyHSV(saturation: 3);

        Assert.AreEqual(HSVColor.Create(180,0.6f, 0.2f), result);
    }

    [TestMethod]
    public void MultiplySaturationClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.MultiplyHSV(saturation: 3);

        Assert.AreEqual(HSVColor.Create(180, 1.0f, 0.5f), result);
    }

    [TestMethod]
    public void MultiplyValueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyHSV(value: 3);

        Assert.AreEqual(HSVColor.Create(180, 0.2f,0.6f), result);
    }

    [TestMethod]
    public void MultiplyValueClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.MultiplyHSV(value: 3);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 1.0f), result);
    }

    #endregion

    #region Divide

    [TestMethod]
    public void DivideHueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.DivideHSV(hue: 30);

        Assert.AreEqual(HSVColor.Create(6, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void DivideSaturationTest()
    {
        Core.Color baseColor = HSVColor.Create(180,0.6f,0.6f);
        Core.Color result = baseColor.DivideHSV(saturation: 2);

        Assert.AreEqual(HSVColor.Create(180, 0.3f,0.6f), result);
    }

    [TestMethod]
    public void DivideValueTest()
    {
        Core.Color baseColor = HSVColor.Create(180,0.6f,0.6f);
        Core.Color result = baseColor.DivideHSV(value: 2);

        Assert.AreEqual(HSVColor.Create(180,0.6f, 0.3f), result);
    }

    #endregion

    #region Set

    [TestMethod]
    public void SetHueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(hue: 30);

        Assert.AreEqual(HSVColor.Create(30, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SetHueWrapTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(hue: 440);

        Assert.AreEqual(HSVColor.Create(80, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SetHueWrapNegativeTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(hue: -30);

        Assert.AreEqual(HSVColor.Create(330, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SetSaturationTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(saturation: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.3f, 0.5f), result);
    }

    [TestMethod]
    public void SetSaturationClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(saturation: 2);

        Assert.AreEqual(HSVColor.Create(180, 1.0f, 0.5f), result);
    }

    [TestMethod]
    public void SetValueTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(value: 0.3f);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 0.3f), result);
    }

    [TestMethod]
    public void SetValueClampTest()
    {
        Core.Color baseColor = HSVColor.Create(180, 0.5f, 0.5f);
        Core.Color result = baseColor.SetHSV(value: 2);

        Assert.AreEqual(HSVColor.Create(180, 0.5f, 1.0f), result);
    }

    #endregion

    #endregion
}