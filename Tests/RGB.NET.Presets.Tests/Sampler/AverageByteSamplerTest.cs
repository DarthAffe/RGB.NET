using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Tests.Sampler;

[TestClass]
public class AverageByteSamplerTest
{
    #region Methods

    [TestMethod]
    public void WhiteTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        colorData.Fill(new Color(1f, 1f, 1f, 1f));
        byte[] result = new byte[4];

        Span<byte> data = new byte[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].GetA();
            data[index++] = colorData[i].GetR();
            data[index++] = colorData[i].GetG();
            data[index++] = colorData[i].GetB();
        }

        SamplerInfo<byte> info = new(0, 0, 2, 3, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 1f, 1f, 1f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 13, 13, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 1f, 1f, 1f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 16, 16, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 1f, 1f, 1f), new Color(result[0], result[1], result[2], result[3]));
    }

    [TestMethod]
    public void BlackTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        colorData.Fill(new Color(1f, 0f, 0f, 0f));
        byte[] result = new byte[4];

        Span<byte> data = new byte[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].GetA();
            data[index++] = colorData[i].GetR();
            data[index++] = colorData[i].GetG();
            data[index++] = colorData[i].GetB();
        }

        SamplerInfo<byte> info = new(0, 0, 2, 3, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0f, 0f, 0f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 13, 13, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0f, 0f, 0f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 16, 16, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0f, 0f, 0f), new Color(result[0], result[1], result[2], result[3]));
    }

    [TestMethod]
    public void GrayTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        for (int i = 0; i < colorData.Length; i++)
            colorData[i] = (i % 2) == 0 ? new Color(1f, 0f, 0f, 0f) : new Color(1f, 1f, 1f, 1f);
        byte[] result = new byte[4];

        Span<byte> data = new byte[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].GetA();
            data[index++] = colorData[i].GetR();
            data[index++] = colorData[i].GetG();
            data[index++] = colorData[i].GetB();
        }

        SamplerInfo<byte> info = new(0, 0, 2, 3, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0.5f, 0.5f, 0.5f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 13, 13, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, (6f / 13f).GetByteValueFromPercentage(), (6f / 13f).GetByteValueFromPercentage(), (6f / 13f).GetByteValueFromPercentage()), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 16, 16, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0.5f, 0.5f, 0.5f), new Color(result[0], result[1], result[2], result[3]));
    }

    [TestMethod]
    public void MixedTest()
    {
        Color[] colorData = new Color[16 * 16];
        for (int i = 0; i < colorData.Length; i++)
            colorData[i] = (i % 5) switch
            {
                0 => new Color(1f, 1f, 0f, 0f),
                1 => new Color(1f, 0f, 0.75f, 0f),
                2 => new Color(0.5f, 0f, 0f, 0.5f),
                3 => new Color(0f, 1f, 1f, 1f),
                _ => new Color(0f, 0f, 0f, 0f),
            };
        byte[] result = new byte[4];

        Span<byte> data = new byte[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].GetA();
            data[index++] = colorData[i].GetR();
            data[index++] = colorData[i].GetG();
            data[index++] = colorData[i].GetB();
        }

        SamplerInfo<byte> info = new(0, 0, 2, 3, 2, 4, data[..(6 * 4)]);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(149, 128, 74, 64), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<byte>(0, 0, 16, 16, 16, 4, data);
        new AverageByteSampler().Sample(info, result);
        Assert.AreEqual(new Color(128, 103, 89, 76), new Color(result[0], result[1], result[2], result[3]));
    }

    #endregion
}