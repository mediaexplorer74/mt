// Decompiled with JetBrains decompiler
// Type: Steamworks.ComputeNewPlayerCompatibilityResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(211)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ComputeNewPlayerCompatibilityResult_t
  {
    public const int k_iCallback = 211;
    public EResult m_eResult;
    public int m_cPlayersThatDontLikeCandidate;
    public int m_cPlayersThatCandidateDoesntLike;
    public int m_cClanPlayersThatDontLikeCandidate;
    public CSteamID m_SteamIDCandidate;
  }
}
