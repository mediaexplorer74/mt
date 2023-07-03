// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.FnaIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

namespace ReLogic.Localization.IME
{
  internal class FnaIme : PlatformIme
  {
    private bool _disposedValue;

    private void OnCharCallback(char key)
    {
      if (!this.IsEnabled || this.OnKeyPress == null)
        return;
      this.OnKeyPress(key);
    }

    public override uint CandidateCount => 0;

    public override string CompositionString => string.Empty;

    public override bool IsCandidateListVisible => false;

    public override uint SelectedCandidate => 0;

    public override string GetCandidate(uint index) => string.Empty;

    protected override void Dispose(bool disposing)
    {
      if (this._disposedValue)
        return;
      int num = disposing ? 1 : 0;
      if (this.IsEnabled)
        this.Disable();
      this._disposedValue = true;
    }

    ~FnaIme() => this.Dispose(false);
  }
}
