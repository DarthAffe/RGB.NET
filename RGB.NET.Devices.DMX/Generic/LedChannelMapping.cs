using System;
using System.Collections;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.DMX;

internal sealed class LedChannelMapping(List<(int channel, Func<Color, byte> getValue)> mappings)
    : IEnumerable<(int channel, Func<Color, byte> getValue)>
{
    #region Properties & Fields

    private readonly List<(int channel, Func<Color, byte> getValue)> _mappings = [..mappings];

    #endregion

    #region Methods

    public IEnumerator<(int channel, Func<Color, byte> getValue)> GetEnumerator() => _mappings.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}