// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageGetPublishedItemVoteDetailsResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1320)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
  {
    public const int k_iCallback = 1320;
    public EResult m_eResult;
    public PublishedFileId_t m_unPublishedFileId;
    public int m_nVotesFor;
    public int m_nVotesAgainst;
    public int m_nReports;
    public float m_fScore;
  }
}
