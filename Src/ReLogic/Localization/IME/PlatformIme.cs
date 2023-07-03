// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.PlatformIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;

namespace ReLogic.Localization.IME
{
  public abstract class PlatformIme : IDisposable
  {
    public Action<char> OnKeyPress;
    private bool _disposedValue;

    public abstract string CompositionString { get; }

    public abstract bool IsCandidateListVisible { get; }

    public abstract uint SelectedCandidate { get; }

    public abstract uint CandidateCount { get; }

    public bool IsEnabled { get; private set; }

    public PlatformIme() => this.IsEnabled = false;

    public abstract string GetCandidate(uint index);

    public void Enable()
    {
      if (this.IsEnabled)
        return;
      this.OnEnable();
      this.IsEnabled = true;
    }

    public void Disable()
    {
      if (!this.IsEnabled)
        return;
      this.OnDisable();
      this.IsEnabled = false;
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposedValue)
        return;
      int num = disposing ? 1 : 0;
      this._disposedValue = true;
    }

    ~PlatformIme() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
