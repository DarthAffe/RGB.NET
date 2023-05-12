using System;

namespace RGB.NET.Core;

public sealed class ActionDisposable : IDisposable
{
    #region Properties & Fields

    private readonly Action _onDispose;

    #endregion

    #region Constructors

    public ActionDisposable(Action onDispose)
    {
        this._onDispose = onDispose;
    }

    #endregion

    #region Methods

    public void Dispose() => _onDispose();

    #endregion
}