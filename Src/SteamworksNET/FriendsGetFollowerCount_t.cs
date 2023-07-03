// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsGetFollowerCount_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(344)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendsGetFollowerCount_t
  {
    public const int k_iCallback = 344;
    public EResult m_eResult;
    public CSteamID m_steamID;
    public int m_nCount;
  }
}
