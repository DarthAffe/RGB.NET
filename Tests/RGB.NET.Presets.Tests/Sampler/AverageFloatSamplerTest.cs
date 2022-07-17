using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Tests.Sampler;

[TestClass]
public class AverageFloatSamplerTest
{
    #region Methods

    [TestMethod]
    public void WhiteTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        colorData.Fill(new Color(1f, 1f, 1f, 1f));
        float[] result = new float[4];

        Span<float> data = new float[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].A;
            data[index++] = colorData[i].R;
            data[index++] = colorData[i].G;
            data[index++] = colorData[i].B;
        }

        SamplerInfo<float> info = new(2, 3, data[..(6 * 4)]);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 1f, 1f, 1f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<float>(16, 16, data);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 1f, 1f, 1f), new Color(result[0], result[1], result[2], result[3]));
    }

    [TestMethod]
    public void BlackTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        colorData.Fill(new Color(1f, 0f, 0f, 0f));
        float[] result = new float[4];

        Span<float> data = new float[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].A;
            data[index++] = colorData[i].R;
            data[index++] = colorData[i].G;
            data[index++] = colorData[i].B;
        }

        SamplerInfo<float> info = new(2, 3, data[..(6 * 4)]);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0f, 0f, 0f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<float>(16, 16, data);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0f, 0f, 0f), new Color(result[0], result[1], result[2], result[3]));
    }

    [TestMethod]
    public void GrayTest()
    {
        Span<Color> colorData = new Color[16 * 16];
        for (int i = 0; i < colorData.Length; i++)
            colorData[i] = (i % 2) == 0 ? new Color(1f, 0f, 0f, 0f) : new Color(1f, 1f, 1f, 1f);
        float[] result = new float[4];

        Span<float> data = new float[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].A;
            data[index++] = colorData[i].R;
            data[index++] = colorData[i].G;
            data[index++] = colorData[i].B;
        }

        SamplerInfo<float> info = new(2, 3, data[..(6 * 4)]);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(1f, 0.5f, 0.5f, 0.5f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<float>(16, 16, data);
        new AverageFloatSampler().Sample(info, result);
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
        float[] result = new float[4];

        Span<float> data = new float[colorData.Length * 4];
        int index = 0;
        for (int i = 0; i < colorData.Length; i++)
        {
            data[index++] = colorData[i].A;
            data[index++] = colorData[i].R;
            data[index++] = colorData[i].G;
            data[index++] = colorData[i].B;
        }

        SamplerInfo<float> info = new(2, 3, data[..(6 * 4)]);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(0.5833333f, 0.5f, 0.291666657f, 0.25f), new Color(result[0], result[1], result[2], result[3]));

        info = new SamplerInfo<float>(16, 16, data);
        new AverageFloatSampler().Sample(info, result);
        Assert.AreEqual(new Color(0.5019531f, 0.40234375f, 0.3486328f, 0.298828125f), new Color(result[0], result[1], result[2], result[3]));
    }

    #endregion
}