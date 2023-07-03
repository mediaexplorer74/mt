// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_LinkAtPosition_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4513)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_LinkAtPosition_t
  {
    public const int k_iCallback = 4513;
    public HHTMLBrowser unBrowserHandle;
    public uint x;
    public uint y;
    public string pchURL;
    [MarshalAs(UnmanagedType.I1)]
    public bool bInput;
    [MarshalAs(UnmanagedType.I1)]
    public bool bLiveLink;
  }
}
