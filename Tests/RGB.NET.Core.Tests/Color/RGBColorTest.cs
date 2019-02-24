using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Color
{
    [TestClass]
    public class RGBColorTest
    {
        #region Manipulation

        #region Blend

        [TestMethod]
        public void BlendOpaqueTest()
        {
            Core.Color baseColor = new Core.Color(255, 0, 0);
            Core.Color blendColor = new Core.Color(0, 255, 0);

            Assert.AreEqual(blendColor, baseColor.Blend(blendColor));
        }

        [TestMethod]
        public void BlendTransparentTest()
        {
            Core.Color baseColor = new Core.Color(255, 0, 0);
            Core.Color blendColor = new Core.Color(0, 0, 255, 0);

            Assert.AreEqual(baseColor, baseColor.Blend(blendColor));
        }

        [TestMethod]
        public void BlendUpTest()
        {
            Core.Color baseColor = new Core.Color(0.0, 0.0, 0.0);
            Core.Color blendColor = new Core.Color(0.5, 1.0, 1.0, 1.0);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.5), baseColor.Blend(blendColor));
        }

        [TestMethod]
        public void BlendDownTest()
        {
            Core.Color baseColor = new Core.Color(1.0, 1.0, 1.0);
            Core.Color blendColor = new Core.Color(1.0, 0.0, 0.0, 0.0);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.5), baseColor.Blend(blendColor));
        }

        #endregion

        #region Add

        [TestMethod]
        public void AddRGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddRGB(11, 12, 13);

            Assert.AreEqual(new Core.Color(128, 139, 140, 141), result);
        }

        [TestMethod]
        public void AddARGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddRGB(10, 11, 12, 13);

            Assert.AreEqual(new Core.Color(138, 139, 140, 141), result);
        }

        [TestMethod]
        public void AddRGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddPercent(0.2, 0.3, 0.4);

            Assert.AreEqual(new Core.Color(0.5, 0.7, 0.8, 0.9), result);
        }

        [TestMethod]
        public void AddARGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddPercent(0.1, 0.2, 0.3, 0.4);

            Assert.AreEqual(new Core.Color(0.6, 0.7, 0.8, 0.9), result);
        }

        [TestMethod]
        public void AddATest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddA(10);

            Assert.AreEqual(new Core.Color(138, 128, 128, 128), result);
        }

        [TestMethod]
        public void AddAPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddAPercent(0.1);

            Assert.AreEqual(new Core.Color(0.6, 0.5, 0.5, 0.5), result);
        }

        [TestMethod]
        public void AddRTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddR(10);

            Assert.AreEqual(new Core.Color(128, 138, 128, 128), result);
        }

        [TestMethod]
        public void AddRPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddRPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.6, 0.5, 0.5), result);
        }

        [TestMethod]
        public void AddGTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddG(10);

            Assert.AreEqual(new Core.Color(128, 128, 138, 128), result);
        }

        [TestMethod]
        public void AddGPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddGPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.6, 0.5), result);
        }

        [TestMethod]
        public void AddBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.AddB(10);

            Assert.AreEqual(new Core.Color(128, 128, 128, 138), result);
        }

        [TestMethod]
        public void AddBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.AddBPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.5, 0.6), result);
        }

        #endregion

        #region Substract

        [TestMethod]
        public void SubtractRGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractRGB(11, 12, 13);

            Assert.AreEqual(new Core.Color(128, 117, 116, 115), result);
        }

        [TestMethod]
        public void SubtractARGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractRGB(10, 11, 12, 13);

            Assert.AreEqual(new Core.Color(118, 117, 116, 115), result);
        }

        [TestMethod]
        public void SubtractRGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractPercent(0.2, 0.3, 0.4);

            Assert.AreEqual(new Core.Color(0.5, 0.3, 0.2, 0.1), result);
        }

        [TestMethod]
        public void SubtractARGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractPercent(0.1, 0.2, 0.3, 0.4);

            Assert.AreEqual(new Core.Color(0.4, 0.3, 0.2, 0.1), result);
        }

        [TestMethod]
        public void SubtractATest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractA(10);

            Assert.AreEqual(new Core.Color(118, 128, 128, 128), result);
        }

        [TestMethod]
        public void SubtractAPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractAPercent(0.1);

            Assert.AreEqual(new Core.Color(0.4, 0.5, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SubtractRTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractR(10);

            Assert.AreEqual(new Core.Color(128, 118, 128, 128), result);
        }

        [TestMethod]
        public void SubtractRPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractRPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.4, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SubtractGTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractG(10);

            Assert.AreEqual(new Core.Color(128, 128, 118, 128), result);
        }

        [TestMethod]
        public void SubtractGPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractGPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.4, 0.5), result);
        }

        [TestMethod]
        public void SubtractBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SubtractB(10);

            Assert.AreEqual(new Core.Color(128, 128, 128, 118), result);
        }

        [TestMethod]
        public void SubtractBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SubtractBPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.5, 0.4), result);
        }

        #endregion

        #region Multiply

        [TestMethod]
        public void MultiplyRGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyPercent(3, 4, 5);

            Assert.AreEqual(new Core.Color(0.2, 0.6, 0.8, 1.0), result);
        }

        [TestMethod]
        public void MultiplyARGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyPercent(2, 3, 4, 5);

            Assert.AreEqual(new Core.Color(0.4, 0.6, 0.8, 1.0), result);
        }

        [TestMethod]
        public void MultiplyAPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyAPercent(3);

            Assert.AreEqual(new Core.Color(0.6, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void MultiplyRPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyRPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.6, 0.2, 0.2), result);
        }

        [TestMethod]
        public void MultiplyGPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyGPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.6, 0.2), result);
        }

        [TestMethod]
        public void MultiplyBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyBPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.6), result);
        }

        #endregion

        #region Divide

        [TestMethod]
        public void DivideRGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.6, 0.8, 1.0);
            Core.Color result = baseColor.DividePercent(3, 4, 5);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void DivideARGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.4, 0.6, 0.8, 1.0);
            Core.Color result = baseColor.DividePercent(2, 3, 4, 5);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void DivideAPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.6, 0.2, 0.2, 0.2);
            Core.Color result = baseColor.DivideAPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void DivideRPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.6, 0.2, 0.2);
            Core.Color result = baseColor.DivideRPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void DivideGPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.6, 0.2);
            Core.Color result = baseColor.DivideGPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        [TestMethod]
        public void DivideBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.2, 0.2, 0.2, 0.6);
            Core.Color result = baseColor.DivideBPercent(3);

            Assert.AreEqual(new Core.Color(0.2, 0.2, 0.2, 0.2), result);
        }

        #endregion

        #region Set

        [TestMethod]
        public void SetRGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetR(11).SetG(12).SetB(13);

            Assert.AreEqual(new Core.Color(128, 11, 12, 13), result);
        }

        [TestMethod]
        public void SetARGBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetA(10).SetR(11).SetG(12).SetB(13);

            Assert.AreEqual(new Core.Color(10, 11, 12, 13), result);
        }

        [TestMethod]
        public void SetRGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetRPercent(0.2).SetGPercent(0.3).SetBPercent(0.4);

            Assert.AreEqual(new Core.Color(0.5, 0.2, 0.3, 0.4), result);
        }

        [TestMethod]
        public void SetARGBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetAPercent(0.1).SetRPercent(0.2).SetGPercent(0.3).SetBPercent(0.4);

            Assert.AreEqual(new Core.Color(0.1, 0.2, 0.3, 0.4), result);
        }

        [TestMethod]
        public void SetATest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetA(10);

            Assert.AreEqual(new Core.Color(10, 128, 128, 128), result);
        }

        [TestMethod]
        public void SetAPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetAPercent(0.1);

            Assert.AreEqual(new Core.Color(0.1, 0.5, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SetRTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetR(10);

            Assert.AreEqual(new Core.Color(128, 10, 128, 128), result);
        }

        [TestMethod]
        public void SetRPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetRPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.1, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SetGTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetG(10);

            Assert.AreEqual(new Core.Color(128, 128, 10, 128), result);
        }

        [TestMethod]
        public void SetGPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetGPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.1, 0.5), result);
        }

        [TestMethod]
        public void SetBTest()
        {
            Core.Color baseColor = new Core.Color(128, 128, 128, 128);
            Core.Color result = baseColor.SetB(10);

            Assert.AreEqual(new Core.Color(128, 128, 128, 10), result);
        }

        [TestMethod]
        public void SetBPercentTest()
        {
            Core.Color baseColor = new Core.Color(0.5, 0.5, 0.5, 0.5);
            Core.Color result = baseColor.SetBPercent(0.1);

            Assert.AreEqual(new Core.Color(0.5, 0.5, 0.5, 0.1), result);
        }

        #endregion

        #endregion
    }
}
