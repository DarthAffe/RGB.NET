using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RGB.NET.Core.Tests.Sampler;

[TestClass]
public class AverageColorSamplerTest
{
    #region Methods

    [TestMethod]
    public void WhiteTest()
    {
        Span<Core.Color> data = new Core.Color[16 * 16];
        data.Fill(new Core.Color(1f, 1f, 1f, 1f));
        Core.Color[] result = new Core.Color[1];

        SamplerInfo<Core.Color> info = new(0, 0, 2, 3, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 1f, 1f, 1f), result[0]);

        info = new SamplerInfo<Core.Color>(0, 0, 16, 16, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 1f, 1f, 1f), result[0]);
    }

    [TestMethod]
    public void BlackTest()
    {
        Span<Core.Color> data = new Core.Color[16 * 16];
        data.Fill(new Core.Color(1f, 0f, 0f, 0f));
        Core.Color[] result = new Core.Color[1];

        SamplerInfo<Core.Color> info = new(0, 0, 2, 3, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 0f, 0f, 0f), result[0]);

        info = new SamplerInfo<Core.Color>(0, 0, 16, 16, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 0f, 0f, 0f), result[0]);
    }

    [TestMethod]
    public void GrayTest()
    {
        Span<Core.Color> data = new Core.Color[16 * 16];
        for (int i = 0; i < data.Length; i++)
            data[i] = (i % 2) == 0 ? new Core.Color(1f, 0f, 0f, 0f) : new Core.Color(1f, 1f, 1f, 1f);
        Core.Color[] result = new Core.Color[1];

        SamplerInfo<Core.Color> info = new(0, 0, 2, 3, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 0.5f, 0.5f, 0.5f), result[0]);

        info = new SamplerInfo<Core.Color>(0, 0, 16, 16, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(1f, 0.5f, 0.5f, 0.5f), result[0]);
    }

    [TestMethod]
    public void MixedTest()
    {
        Core.Color[] data = new Core.Color[16 * 16];
        for (int i = 0; i < data.Length; i++)
            data[i] = (i % 5) switch
            {
                0 => new Core.Color(1f, 1f, 0f, 0f),
                1 => new Core.Color(1f, 0f, 0.75f, 0f),
                2 => new Core.Color(0.5f, 0f, 0f, 0.5f),
                3 => new Core.Color(0f, 1f, 1f, 1f),
                _ => new Core.Color(0f, 0f, 0f, 0f),
            };
        Core.Color[] result = new Core.Color[1];

        SamplerInfo<Core.Color> info = new(0, 0, 2, 3, 2, 1, data[..6]);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(0.5833333f, 0.5f, 0.291666657f, 0.25f), result[0]);

        info = new SamplerInfo<Core.Color>(0, 0, 16, 16, 16, 1, data);
        new AverageColorSampler().Sample(info, result);
        Assert.AreEqual(new Core.Color(0.5019531f, 0.40234375f, 0.3486328f, 0.298828125f), result[0]);
    }

    #endregion
}