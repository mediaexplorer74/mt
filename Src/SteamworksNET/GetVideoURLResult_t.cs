// Decompiled with JetBrains decompiler
// Type: Steamworks.GetVideoURLResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4611)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GetVideoURLResult_t
  {
    public const int k_iCallback = 4611;
    public EResult m_eResult;
    public AppId_t m_unVideoAppID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchURL;
  }
}
