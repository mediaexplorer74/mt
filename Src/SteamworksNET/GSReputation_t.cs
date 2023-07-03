// Decompiled with JetBrains decompiler
// Type: Steamworks.GSReputation_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(209)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GSReputation_t
  {
    public const int k_iCallback = 209;
    public EResult m_eResult;
    public uint m_unReputationScore;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bBanned;
    public uint m_unBannedIP;
    public ushort m_usBannedPort;
    public ulong m_ulBannedGameID;
    public uint m_unBanExpires;
  }
}
