using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Color;

[TestClass]
public class RGBColorTest
{
    #region Manipulation

    #region Blend

    [TestMethod]
    public void BlendOpaqueTest()
    {
        Core.Color baseColor = new(255, 0, 0);
        Core.Color blendColor = new(0, 255, 0);

        Assert.AreEqual(blendColor, baseColor.Blend(blendColor));
    }

    [TestMethod]
    public void BlendTransparentTest()
    {
        Core.Color baseColor = new(255, 0, 0);
        Core.Color blendColor = new(0, 0, 255, 0);

        Assert.AreEqual(baseColor, baseColor.Blend(blendColor));
    }

    [TestMethod]
    public void BlendUpTest()
    {
        Core.Color baseColor = new(0.0f, 0.0f, 0.0f);
        Core.Color blendColor = new(0.5f, 1.0f, 1.0f, 1.0f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.5f), baseColor.Blend(blendColor));
    }

    [TestMethod]
    public void BlendDownTest()
    {
        Core.Color baseColor = new(1.0f, 1.0f, 1.0f);
        Core.Color blendColor = new(0.5f, 0.0f, 0.0f, 0.0f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.5f), baseColor.Blend(blendColor));
    }

    #endregion

    #region Add

    [TestMethod]
    public void AddRGBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.AddRGB(11, 12, 13);

        Assert.AreEqual(new Core.Color(128, 139, 140, 141), result);
    }

    [TestMethod]
    public void AddRGBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.AddRGB(0.2f, 0.3f, 0.4f);

        Assert.AreEqual(new Core.Color(0.5f, 0.7f, 0.8f, 0.9f), result);
    }

    [TestMethod]
    public void AddATest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.AddA(10);

        Assert.AreEqual(new Core.Color(138, 128, 128, 128), result);
    }

    [TestMethod]
    public void AddAPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.AddA(0.1f);

        Assert.AreEqual(new Core.Color(0.6f, 0.5f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void AddRTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.AddRGB(r: 10);

        Assert.AreEqual(new Core.Color(128, 138, 128, 128), result);
    }

    [TestMethod]
    public void AddRPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.AddRGB(r: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.6f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void AddGTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.AddRGB(g: 10);

        Assert.AreEqual(new Core.Color(128, 128, 138, 128), result);
    }

    [TestMethod]
    public void AddGPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.AddRGB(g: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.6f, 0.5f), result);
    }

    [TestMethod]
    public void AddBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.AddRGB(b: 10);

        Assert.AreEqual(new Core.Color(128, 128, 128, 138), result);
    }

    [TestMethod]
    public void AddBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.AddRGB(b: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.5f, 0.6f), result);
    }

    #endregion

    #region Substract

    [TestMethod]
    public void SubtractRGBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SubtractRGB(11, 12, 13);

        Assert.AreEqual(new Core.Color(128, 117, 116, 115), result);
    }

    [TestMethod]
    public void SubtractRGBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractRGB(0.2f, 0.3f, 0.4f);

        Assert.AreEqual(new Core.Color(0.5f, 0.3f, 0.2f, 0.1f), result);
    }

    [TestMethod]
    public void SubtractATest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SubtractA(10);

        Assert.AreEqual(new Core.Color(118, 128, 128, 128), result);
    }

    [TestMethod]
    public void SubtractAPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractA(0.1f);

        Assert.AreEqual(new Core.Color(0.4f, 0.5f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractRTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SubtractRGB(r: 10);

        Assert.AreEqual(new Core.Color(128, 118, 128, 128), result);
    }

    [TestMethod]
    public void SubtractRPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractRGB(r: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.4f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractGTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SubtractRGB(g: 10);

        Assert.AreEqual(new Core.Color(128, 128, 118, 128), result);
    }

    [TestMethod]
    public void SubtractGPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractRGB(g: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.4f, 0.5f), result);
    }

    [TestMethod]
    public void SubtractBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SubtractRGB(b: 10);

        Assert.AreEqual(new Core.Color(128, 128, 128, 118), result);
    }

    [TestMethod]
    public void SubtractBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SubtractRGB(b: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.5f, 0.4f), result);
    }

    #endregion

    #region Multiply

    [TestMethod]
    public void MultiplyRGBPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyRGB(3, 4, 5);

        Assert.AreEqual(new Core.Color(0.2f, 0.6f, 0.8f, 1.0f), result);
    }

    [TestMethod]
    public void MultiplyAPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyA(3);

        Assert.AreEqual(new Core.Color(0.6f, 0.2f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void MultiplyRPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyRGB(r: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.6f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void MultiplyGPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyRGB(g: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.6f, 0.2f), result);
    }

    [TestMethod]
    public void MultiplyBPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.MultiplyRGB(b: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.6f), result);
    }

    #endregion

    #region Divide

    [TestMethod]
    public void DivideRGBPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.6f, 0.8f, 1.0f);
        Core.Color result = baseColor.DivideRGB(3, 4, 5);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void DivideAPercentTest()
    {
        Core.Color baseColor = new(0.6f, 0.2f, 0.2f, 0.2f);
        Core.Color result = baseColor.DivideA(3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void DivideRPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.6f, 0.2f, 0.2f);
        Core.Color result = baseColor.DivideRGB(r: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void DivideGPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.6f, 0.2f);
        Core.Color result = baseColor.DivideRGB(g: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.2f), result);
    }

    [TestMethod]
    public void DivideBPercentTest()
    {
        Core.Color baseColor = new(0.2f, 0.2f, 0.2f, 0.6f);
        Core.Color result = baseColor.DivideRGB(b: 3);

        Assert.AreEqual(new Core.Color(0.2f, 0.2f, 0.2f, 0.2f), result);
    }

    #endregion

    #region Set

    [TestMethod]
    public void SetRGBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SetRGB(11, 12, 13);

        Assert.AreEqual(new Core.Color(128, 11, 12, 13), result);
    }

    [TestMethod]
    public void SetRGBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SetRGB(0.2f, 0.3f, 0.4f);

        Assert.AreEqual(new Core.Color(0.5f, 0.2f, 0.3f, 0.4f), result);
    }

    [TestMethod]
    public void SetATest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SetA(10);

        Assert.AreEqual(new Core.Color(10, 128, 128, 128), result);
    }

    [TestMethod]
    public void SetAPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SetA(0.1f);

        Assert.AreEqual(new Core.Color(0.1f, 0.5f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SetRTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SetRGB(r: 10);

        Assert.AreEqual(new Core.Color(128, 10, 128, 128), result);
    }

    [TestMethod]
    public void SetRPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SetRGB(r: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.1f, 0.5f, 0.5f), result);
    }

    [TestMethod]
    public void SetGTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SetRGB(g: 10);

        Assert.AreEqual(new Core.Color(128, 128, 10, 128), result);
    }

    [TestMethod]
    public void SetGPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SetRGB(g: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.1f, 0.5f), result);
    }

    [TestMethod]
    public void SetBTest()
    {
        Core.Color baseColor = new(128, 128, 128, 128);
        Core.Color result = baseColor.SetRGB(b: 10);

        Assert.AreEqual(new Core.Color(128, 128, 128, 10), result);
    }

    [TestMethod]
    public void SetBPercentTest()
    {
        Core.Color baseColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        Core.Color result = baseColor.SetRGB(b: 0.1f);

        Assert.AreEqual(new Core.Color(0.5f, 0.5f, 0.5f, 0.1f), result);
    }

    #endregion

    #endregion
}