using System;

namespace RGB.NET.Core;

public sealed class ActionDisposable(Action onDispose) : IDisposable
{
    #region Methods

    public void Dispose() => onDispose();

    #endregion
}