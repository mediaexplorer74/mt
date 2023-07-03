// Decompiled with JetBrains decompiler
// Type: Steamworks.GSClientGroupStatus_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(208)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct GSClientGroupStatus_t
  {
    public const int k_iCallback = 208;
    public CSteamID m_SteamIDUser;
    public CSteamID m_SteamIDGroup;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bMember;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bOfficer;
  }
}
