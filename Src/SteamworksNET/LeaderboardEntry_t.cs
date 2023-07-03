// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardEntry_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardEntry_t
  {
    public CSteamID m_steamIDUser;
    public int m_nGlobalRank;
    public int m_nScore;
    public int m_cDetails;
    public UGCHandle_t m_hUGC;
  }
}
