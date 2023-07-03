// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.FnaPlatform
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using ReLogic.Localization.IME;
using System;

namespace ReLogic.OS
{
  internal abstract class FnaPlatform : Platform
  {
    public FnaPlatform(PlatformType type)
      : base(type)
    {
    }

    protected override PlatformIme CreateIme(IntPtr windowHandle) => (PlatformIme) new FnaIme();
  }
}
