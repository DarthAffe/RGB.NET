using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Color
{
    [TestClass]
    public class HSVColorTest
    {
        #region Manipulation

        #region Blend

        [TestMethod]
        public void BlendOpaqueTest()
        {
            Assert.Inconclusive();
            //Core.Color baseColor = new Core.Color(255, 0, 0);
            //Core.Color blendColor = new Core.Color(0, 255, 0);

            //Assert.AreEqual(blendColor, baseColor.Blend(blendColor));
        }

        [TestMethod]
        public void BlendTransparentTest()
        {
            Assert.Inconclusive();
            //Core.Color baseColor = new Core.Color(255, 0, 0);
            //Core.Color blendColor = new Core.Color(0, 0, 255, 0);

            //Assert.AreEqual(baseColor, baseColor.Blend(blendColor));
        }

        #endregion

        #region Add

        [TestMethod]
        public void AddHueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddHue(30);

            Assert.AreEqual(Core.Color.FromHSV(210, 0.5, 0.5), result);
        }

        [TestMethod]
        public void AddHueWrapTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddHue(220);

            Assert.AreEqual(Core.Color.FromHSV(40, 0.5, 0.5), result);
        }

        [TestMethod]
        public void AddSaturationTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddSaturation(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.8, 0.5), result);
        }

        [TestMethod]
        public void AddSaturationClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddSaturation(0.8);

            Assert.AreEqual(Core.Color.FromHSV(180, 1.0, 0.5), result);
        }

        [TestMethod]
        public void AddValueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddValue(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 0.8), result);
        }

        [TestMethod]
        public void AddValueClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.AddValue(0.8);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 1.0), result);
        }

        #endregion

        #region Subtract

        [TestMethod]
        public void SubtractHueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractHue(30);

            Assert.AreEqual(Core.Color.FromHSV(150, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SubtractHueWrapTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractHue(220);

            Assert.AreEqual(Core.Color.FromHSV(320, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SubtractSaturationTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractSaturation(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.2, 0.5), result);
        }

        [TestMethod]
        public void SubtractSaturationClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractSaturation(0.8);

            Assert.AreEqual(Core.Color.FromHSV(180, 0, 0.5), result);
        }

        [TestMethod]
        public void SubtractValueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractValue(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 0.2), result);
        }

        [TestMethod]
        public void SubtractValueClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SubtractValue(0.8);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 0), result);
        }

        #endregion

        #region Multiply

        [TestMethod]
        public void MultiplyHueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.MultiplyHue(1.5);

            Assert.AreEqual(Core.Color.FromHSV(270, 0.5, 0.5), result);
        }

        [TestMethod]
        public void MultiplyHueWrapTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.MultiplyHue(3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 0.5), result);
        }

        [TestMethod]
        public void MultiplySaturationTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.2, 0.2);
            Core.Color result = baseColor.MultiplySaturation(3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.6, 0.2), result);
        }

        [TestMethod]
        public void MultiplySaturationClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.MultiplySaturation(3);

            Assert.AreEqual(Core.Color.FromHSV(180, 1.0, 0.5), result);
        }

        [TestMethod]
        public void MultiplyValueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.2, 0.2);
            Core.Color result = baseColor.MultiplyValue(3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.2, 0.6), result);
        }

        [TestMethod]
        public void MultiplyValueClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.MultiplyValue(3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 1.0), result);
        }

        #endregion

        #region Divide

        [TestMethod]
        public void DivideHueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.DivideHue(30);

            Assert.AreEqual(Core.Color.FromHSV(6, 0.5, 0.5), result);
        }

        [TestMethod]
        public void DivideSaturationTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.6, 0.6);
            Core.Color result = baseColor.DivideSaturation(2);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.3, 0.6), result);
        }

        [TestMethod]
        public void DivideValueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.6, 0.6);
            Core.Color result = baseColor.DivideValue(2);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.6, 0.3), result);
        }

        #endregion

        #region Set

        [TestMethod]
        public void SetHueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetHue(30);

            Assert.AreEqual(Core.Color.FromHSV(30, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SetHueWrapTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetHue(440);

            Assert.AreEqual(Core.Color.FromHSV(80, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SetHueWrapNegativeTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetHue(-30);

            Assert.AreEqual(Core.Color.FromHSV(330, 0.5, 0.5), result);
        }

        [TestMethod]
        public void SetSaturationTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetSaturation(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.3, 0.5), result);
        }

        [TestMethod]
        public void SetSaturationClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetSaturation(2);

            Assert.AreEqual(Core.Color.FromHSV(180, 1.0, 0.5), result);
        }

        [TestMethod]
        public void SetValueTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetValue(0.3);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 0.3), result);
        }

        [TestMethod]
        public void SetValueClampTest()
        {
            Core.Color baseColor = Core.Color.FromHSV(180, 0.5, 0.5);
            Core.Color result = baseColor.SetValue(2);

            Assert.AreEqual(Core.Color.FromHSV(180, 0.5, 1.0), result);
        }

        #endregion

        #endregion
    }
}
