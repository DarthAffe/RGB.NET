using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RGB.NET.Core.Tests.Helper;

namespace RGB.NET.Core.Tests.Texture;

[TestClass]
public class PixelTextureTest
{
    #region Methods

    [TestMethod]
    public void SampleRegionsTest()
    {
        const int SIZE = 1024;

        Dictionary<Rectangle, Core.Color> testData = new()
        {
            [new Rectangle(0, 0, 1, 1)] = new Core.Color(255, 106, 159, 118),
            [new Rectangle(0.09765625f, 0.486328125f, 0.427734375f, 0.2890625f)] = new Core.Color(255, 86, 115, 175),
            [new Rectangle(0.5859375f, 0.111328125f, 0.271484375f, 0.826171875f)] = new Core.Color(255, 85, 183, 123),
            [new Rectangle(0.279296875f, 0.439453125f, 0.583984375f, 0.499609375f)] = new Core.Color(255, 96, 144, 145),
            [new Rectangle(0.603515625f, 0.646484375f, 0.365234375f, 0.306640625f)] = new Core.Color(255, 92, 151, 141),
            [new Rectangle(0.583984375f, 0.11328125f, 0.314453125f, 0.662109375f)] = new Core.Color(255, 75, 201, 115),
            [new Rectangle(0.166015625f, 0.740234375f, 0.76171875f, 0.166015625f)] = new Core.Color(255, 90, 150, 142),
            [new Rectangle(0.384765625f, 0.017578125f, 0.576171875f, 0.82421875f)] = new Core.Color(255, 94, 164, 128),
            [new Rectangle(0.216796875f, 0.5390625f, 0.669921875f, 0.2890625f)] = new Core.Color(255, 76, 135, 169),
            [new Rectangle(0.08203125f, 0.060546875f, 0.857421875f, 0.8671875f)] = new Core.Color(255, 98, 167, 117),
            [new Rectangle(0.345703125f, 0.431640625f, 0.560546875f, 0.25421875f)] = new Core.Color(255, 106, 167, 116),
            [new Rectangle(0.54296875f, 0.12890625f, 0.40234375f, 0.8515625f)] = new Core.Color(255, 89, 183, 115),
            [new Rectangle(0.00390625f, 0.462890625f, 0.953125f, 0.052734375f)] = new Core.Color(255, 138, 173, 96),
            [new Rectangle(0.322265625f, 0.572265625f, 0.361328125f, 0.40234375f)] = new Core.Color(255, 123, 127, 128),
            [new Rectangle(0.56640625f, 0.388671875f, 0.28125f, 0.423828125f)] = new Core.Color(255, 112, 161, 118),
            [new Rectangle(0.119140625f, 0.28125f, 0.828125f, 0.501953125f)] = new Core.Color(255, 105, 170, 108),
            [new Rectangle(0.173828125f, 0.8359375f, 0.7421875f, 0.119140625f)] = new Core.Color(255, 126, 151, 106),
            [new Rectangle(0.109375f, 0.283203125f, 0.748046875f, 0.583984375f)] = new Core.Color(255, 102, 158, 122),
            [new Rectangle(0.0546875f, 0.474609375f, 0.87109375f, 0.2734375f)] = new Core.Color(255, 101, 143, 140),
            [new Rectangle(0.34765625f, 0.30859375f, 0.39453125f, 0.39453125f)] = new Core.Color(255, 99, 143, 136),
            [new Rectangle(0.240234375f, 0.6796875f, 0.515625f, 0.248046875f)] = new Core.Color(255, 114, 135, 132),
        };

        Core.Color[] data = new Core.Color[SIZE * SIZE];
        SimplexNoise.Seed = 1872;
        Random random = new(1872);
        for (int y = 0; y < SIZE; y++)
            for (int x = 0; x < SIZE; x++)
                data[(y * SIZE) + x] = HSVColor.Create(SimplexNoise.CalcPixel2D(x, y, 1f / SIZE) * 360, 1, 1);

        PixelTexture texture = new(SIZE, SIZE, data);
        foreach ((Rectangle rect, Core.Color color) in testData)
        {
            // DarthAffe 23.04.2023: To check it "correctly" the test-data would need to be setup with floating point colors, but i don't really bother for now - that should be good enough to detect breaking changes
            (byte, byte, byte, byte) sampled = texture[rect].GetRGBBytes();
            (byte, byte, byte, byte) refColor = color.GetRGBBytes();
            Assert.AreEqual(refColor, sampled);
        }
    }

    #endregion
}