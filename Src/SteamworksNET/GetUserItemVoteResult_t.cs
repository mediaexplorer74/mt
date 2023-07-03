// Decompiled with JetBrains decompiler
// Type: Steamworks.GetUserItemVoteResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3409)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GetUserItemVoteResult_t
  {
    public const int k_iCallback = 3409;
    public PublishedFileId_t m_nPublishedFileId;
    public EResult m_eResult;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bVotedUp;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bVotedDown;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bVoteSkipped;
  }
}
