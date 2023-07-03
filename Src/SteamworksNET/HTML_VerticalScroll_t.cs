// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_VerticalScroll_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4512)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_VerticalScroll_t
  {
    public const int k_iCallback = 4512;
    public HHTMLBrowser unBrowserHandle;
    public uint unScrollMax;
    public uint unScrollCurrent;
    public float flPageScale;
    [MarshalAs(UnmanagedType.I1)]
    public bool bVisible;
    public uint unPageSize;
  }
}
