// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoreUploaded_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1106)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardScoreUploaded_t
  {
    public const int k_iCallback = 1106;
    public byte m_bSuccess;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public int m_nScore;
    public byte m_bScoreChanged;
    public int m_nGlobalRankNew;
    public int m_nGlobalRankPrevious;
  }
}
