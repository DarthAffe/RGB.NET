using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Color
{
    [TestClass]
    public class ColorTest
    {
        #region Basics

        [TestMethod]
        public void VerifyTransparent()
        {
            Core.Color transparent = Core.Color.Transparent;

            Assert.AreEqual(0, transparent.GetA(), "A is not 0");
            Assert.AreEqual(0, transparent.GetR(), "R is not 0");
            Assert.AreEqual(0, transparent.GetG(), "G is not 0");
            Assert.AreEqual(0, transparent.GetB(), "B is not 0");
        }

        [TestMethod]
        public void ToStringTest()
        {
            Core.Color color = new Core.Color(255, 120, 13, 1);

            Assert.AreEqual("[A: 255, R: 120, G: 13, B: 1]", color.ToString());
        }

        #region HashCode

        [TestMethod]
        public void GetHashCodeTestEqual()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 32, 255);

            Assert.AreEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTestNotEqualA()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(99, 68, 32, 255);

            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTestNotEqualR()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 69, 32, 255);

            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTestNotEqualG()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 200, 255);

            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTestNotEqualB()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 32, 0);

            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        #endregion

        #region Equality

        [TestMethod]
        public void EqualityTestEqual()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 32, 255);

            Assert.IsTrue(color1.Equals(color2), $"Equals returns false on equal colors {color1} and {color2}");
            Assert.IsTrue(color1 == color2, $"Equal-operator returns false on equal colors {color1} and {color2}");
            Assert.IsFalse(color1 != color2, $"Not-Equal-operator returns true on equal colors {color1} and {color2}");
        }

        [TestMethod]
        public void EqualityTestNotEqualA()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(99, 68, 32, 255);

            Assert.IsFalse(color1.Equals(color2), $"Equals returns true on not equal colors {color1} and {color2}");
            Assert.IsFalse(color1 == color2, $"Equal-operator returns true on not equal colors {color1} and {color2}");
            Assert.IsTrue(color1 != color2, $"Not-Equal-operator returns false on not equal colors {color1} and {color2}");
        }

        [TestMethod]
        public void EqualityTestNotEqualR()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 69, 32, 255);

            Assert.IsFalse(color1.Equals(color2), $"Equals returns true on not equal colors {color1} and {color2}");
            Assert.IsFalse(color1 == color2, $"Equal-operator returns true on not equal colors {color1} and {color2}");
            Assert.IsTrue(color1 != color2, $"Not-Equal-operator returns false on not equal colors {color1} and {color2}");
        }

        [TestMethod]
        public void EqualityTestNotEqualG()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 200, 255);

            Assert.IsFalse(color1.Equals(color2), $"Equals returns true on not equal colors {color1} and {color2}");
            Assert.IsFalse(color1 == color2, $"Equal-operator returns true on not equal colors {color1} and {color2}");
            Assert.IsTrue(color1 != color2, $"Not-Equal-operator returns false on not equal colors {color1} and {color2}");
        }

        [TestMethod]
        public void EqualityTestNotEqualB()
        {
            Core.Color color1 = new Core.Color(100, 68, 32, 255);
            Core.Color color2 = new Core.Color(100, 68, 32, 0);

            Assert.IsFalse(color1.Equals(color2), $"Equals returns true on not equal colors {color1} and {color2}");
            Assert.IsFalse(color1 == color2, $"Equal-operator returns true on not equal colors {color1} and {color2}");
            Assert.IsTrue(color1 != color2, $"Not-Equal-operator returns false on not equal colors {color1} and {color2}");
        }

        #endregion

        #endregion

        #region Constructors

        [TestMethod]
        public void RGBByteConstructorTest()
        {
            Core.Color color = new Core.Color((byte)10, (byte)120, (byte)255);

            Assert.AreEqual(255, color.GetA(), "A is not 255");
            Assert.AreEqual(10, color.GetR(), "R is not 10");
            Assert.AreEqual(120, color.GetG(), "G is not 120");
            Assert.AreEqual(255, color.GetB(), "B is not 255");
        }

        [TestMethod]
        public void ARGBByteConstructorTest()
        {
            Core.Color color = new Core.Color((byte)200, (byte)10, (byte)120, (byte)255);

            Assert.AreEqual(200, color.GetA(), "A is not 200");
            Assert.AreEqual(10, color.GetR(), "R is not 10");
            Assert.AreEqual(120, color.GetG(), "G is not 120");
            Assert.AreEqual(255, color.GetB(), "B is not 255");
        }

        [TestMethod]
        public void RGBIntConstructorTest()
        {
            Core.Color color = new Core.Color(10, 120, 255);

            Assert.AreEqual(255, color.GetA(), "A is not 255");
            Assert.AreEqual(10, color.GetR(), "R is not 10");
            Assert.AreEqual(120, color.GetG(), "G is not 120");
            Assert.AreEqual(255, color.GetB(), "B is not 255");
        }

        [TestMethod]
        public void ARGBIntConstructorTest()
        {
            Core.Color color = new Core.Color(200, 10, 120, 255);

            Assert.AreEqual(200, color.GetA(), "A is not 200");
            Assert.AreEqual(10, color.GetR(), "R is not 10");
            Assert.AreEqual(120, color.GetG(), "G is not 120");
            Assert.AreEqual(255, color.GetB(), "B is not 255");
        }

        [TestMethod]
        public void RGBIntConstructorClampTest()
        {
            Core.Color color1 = new Core.Color(256, 256, 256);

            Assert.AreEqual(255, color1.GetA(), "A is not 255");
            Assert.AreEqual(255, color1.GetR(), "R is not 255");
            Assert.AreEqual(255, color1.GetG(), "G is not 255");
            Assert.AreEqual(255, color1.GetB(), "B is not 255");

            Core.Color color2 = new Core.Color(-1, -1, -1);

            Assert.AreEqual(255, color2.GetA(), "A is not 255");
            Assert.AreEqual(0, color2.GetR(), "R is not 0");
            Assert.AreEqual(0, color2.GetG(), "G is not 0");
            Assert.AreEqual(0, color2.GetB(), "B is not 0");
        }

        [TestMethod]
        public void ARGBIntConstructorClampTest()
        {
            Core.Color color = new Core.Color(256, 256, 256, 256);

            Assert.AreEqual(255, color.GetA(), "A is not 255");
            Assert.AreEqual(255, color.GetR(), "R is not 255");
            Assert.AreEqual(255, color.GetG(), "G is not 255");
            Assert.AreEqual(255, color.GetB(), "B is not 255");

            Core.Color color2 = new Core.Color(-1, -1, -1, -1);

            Assert.AreEqual(0, color2.GetA(), "A is not 0");
            Assert.AreEqual(0, color2.GetR(), "R is not 0");
            Assert.AreEqual(0, color2.GetG(), "G is not 0");
            Assert.AreEqual(0, color2.GetB(), "B is not 0");
        }

        [TestMethod]
        public void RGBPercentConstructorTest()
        {
            Core.Color color = new Core.Color(0.25341, 0.55367, 1);

            Assert.AreEqual(1, color.A, DoubleExtensions.TOLERANCE, "A is not 1");
            Assert.AreEqual(0.25341, color.R, DoubleExtensions.TOLERANCE, "R is not 0.25341");
            Assert.AreEqual(0.55367, color.G, DoubleExtensions.TOLERANCE, "G is not 0.55367");
            Assert.AreEqual(1, color.B, DoubleExtensions.TOLERANCE, "B is not 1");
        }

        [TestMethod]
        public void ARGBPercentConstructorTest()
        {
            Core.Color color = new Core.Color(0.3315, 0.25341, 0.55367, 1);

            Assert.AreEqual(0.3315, color.A, DoubleExtensions.TOLERANCE, "A is not 0.3315");
            Assert.AreEqual(0.25341, color.R, DoubleExtensions.TOLERANCE, "R is not 0.25341");
            Assert.AreEqual(0.55367, color.G, DoubleExtensions.TOLERANCE, "G is not 0.55367");
            Assert.AreEqual(1, color.B, DoubleExtensions.TOLERANCE, "B is not 1");
        }

        [TestMethod]
        public void RGBPercentConstructorClampTest()
        {
            Core.Color color1 = new Core.Color(1.1, 1.1, 1.1);

            Assert.AreEqual(1, color1.A, "A is not 1");
            Assert.AreEqual(1, color1.R, "R is not 1");
            Assert.AreEqual(1, color1.G, "G is not 1");
            Assert.AreEqual(1, color1.B, "B is not 1");

            Core.Color color2 = new Core.Color(-1.0, -1.0, -1.0);

            Assert.AreEqual(1, color2.A, "A is not 1");
            Assert.AreEqual(0, color2.R, "R is not 0");
            Assert.AreEqual(0, color2.G, "G is not 0");
            Assert.AreEqual(0, color2.B, "B is not 0");
        }

        [TestMethod]
        public void ARGBPercentConstructorClampTest()
        {
            Core.Color color1 = new Core.Color(1.1, 1.1, 1.1, 1.1);

            Assert.AreEqual(1, color1.A, "A is not 1");
            Assert.AreEqual(1, color1.R, "R is not 1");
            Assert.AreEqual(1, color1.G, "G is not 1");
            Assert.AreEqual(1, color1.B, "B is not 1");

            Core.Color color2 = new Core.Color(-1.0, -1.0, -1.0, -1.0);

            Assert.AreEqual(0, color2.A, "A is not 0");
            Assert.AreEqual(0, color2.R, "R is not 0");
            Assert.AreEqual(0, color2.G, "G is not 0");
            Assert.AreEqual(0, color2.B, "B is not 0");
        }

        [TestMethod]
        public void CloneConstructorTest()
        {
            Core.Color referennceColor = new Core.Color(200, 10, 120, 255);
            Core.Color color = new Core.Color(referennceColor);

            Assert.AreEqual(200, color.GetA(), "A is not 200");
            Assert.AreEqual(10, color.GetR(), "R is not 10");
            Assert.AreEqual(120, color.GetG(), "G is not 120");
            Assert.AreEqual(255, color.GetB(), "B is not 255");
        }

        #endregion

        #region Conversion

        [TestMethod]
        public void ColorFromComponentsTest()
        {
            Core.Color color = (255, 120, 13, 1);

            Assert.AreEqual(255, color.GetA(), $"A doesn't equal the used component. ({color.GetA()} != 255)");
            Assert.AreEqual(120, color.GetR(), $"R doesn't equal the used component. ({color.GetR()} != 120)");
            Assert.AreEqual(13, color.GetG(), $"G doesn't equal the used component. ({color.GetG()} != 13)");
            Assert.AreEqual(1, color.GetB(), $"B doesn't equal the used component. ({color.GetB()} != 1)");
        }

        [TestMethod]
        public void DesconstructTest()
        {
            (byte a, byte r, byte g, byte b) = new Core.Color(255, 120, 13, 1).GetRGBBytes();

            Assert.AreEqual(255, a, $"A doesn't equal the color. ({a} != 255)");
            Assert.AreEqual(120, r, $"R doesn't equal the color. ({r} != 120)");
            Assert.AreEqual(13, g, $"G doesn't equal the color. ({g} != 13)");
            Assert.AreEqual(1, b, $"B doesn't equal the color. ({b} != 1)");
        }

        [TestMethod]
        public void AToPercentTest()
        {
            Core.Color color1 = new Core.Color(0, 0, 0, 0);
            Assert.AreEqual(0, color1.A);

            Core.Color color2 = new Core.Color(255, 0, 0, 0);
            Assert.AreEqual(1, color2.A);

            Core.Color color3 = new Core.Color(128, 0, 0, 0);
            Assert.AreEqual(128 / 255.0, color3.A);

            Core.Color color4 = new Core.Color(30, 0, 0, 0);
            Assert.AreEqual(30 / 255.0, color4.A);

            Core.Color color5 = new Core.Color(201, 0, 0, 0);
            Assert.AreEqual(201 / 255.0, color5.A);
        }

        [TestMethod]
        public void RToPercentTest()
        {
            Core.Color color1 = new Core.Color(0, 0, 0, 0);
            Assert.AreEqual(0, color1.R);

            Core.Color color2 = new Core.Color(0, 255, 0, 0);
            Assert.AreEqual(1, color2.R);

            Core.Color color3 = new Core.Color(0, 128, 0, 0);
            Assert.AreEqual(128 / 255.0, color3.R);

            Core.Color color4 = new Core.Color(0, 30, 0, 0);
            Assert.AreEqual(30 / 255.0, color4.R);

            Core.Color color5 = new Core.Color(0, 201, 0, 0);
            Assert.AreEqual(201 / 255.0, color5.R);
        }

        [TestMethod]
        public void GToPercentTest()
        {
            Core.Color color1 = new Core.Color(0, 0, 0, 0);
            Assert.AreEqual(0, color1.G);

            Core.Color color2 = new Core.Color(0, 0, 255, 0);
            Assert.AreEqual(1, color2.G);

            Core.Color color3 = new Core.Color(0, 0, 128, 0);
            Assert.AreEqual(128 / 255.0, color3.G);

            Core.Color color4 = new Core.Color(0, 0, 30, 0);
            Assert.AreEqual(30 / 255.0, color4.G);

            Core.Color color5 = new Core.Color(0, 0, 201, 0);
            Assert.AreEqual(201 / 255.0, color5.G);
        }

        [TestMethod]
        public void BToPercentTest()
        {
            Core.Color color1 = new Core.Color(0, 0, 0, 0);
            Assert.AreEqual(0, color1.B);

            Core.Color color2 = new Core.Color(0, 0, 0, 255);
            Assert.AreEqual(1, color2.B);

            Core.Color color3 = new Core.Color(0, 0, 0, 128);
            Assert.AreEqual(128 / 255.0, color3.B);

            Core.Color color4 = new Core.Color(0, 0, 0, 30);
            Assert.AreEqual(30 / 255.0, color4.B);

            Core.Color color5 = new Core.Color(0, 0, 0, 201);
            Assert.AreEqual(201 / 255.0, color5.B);
        }

        #endregion
    }
}
