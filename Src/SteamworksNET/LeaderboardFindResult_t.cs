// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardFindResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1104)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardFindResult_t
  {
    public const int k_iCallback = 1104;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public byte m_bLeaderboardFound;
  }
}
