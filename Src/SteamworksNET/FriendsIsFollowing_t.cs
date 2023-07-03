// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsIsFollowing_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(345)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendsIsFollowing_t
  {
    public const int k_iCallback = 345;
    public EResult m_eResult;
    public CSteamID m_steamID;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bIsFollowing;
  }
}
