// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.UnsupportedPlatformIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

namespace ReLogic.Localization.IME
{
  public class UnsupportedPlatformIme : PlatformIme
  {
    public override uint CandidateCount => 0;

    public override string CompositionString => string.Empty;

    public override bool IsCandidateListVisible => false;

    public override uint SelectedCandidate => 0;

    public override string GetCandidate(uint index) => string.Empty;
  }
}
