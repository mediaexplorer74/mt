// Decompiled with JetBrains decompiler
// Type: Steamworks.GlobalStatsReceived_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1112)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GlobalStatsReceived_t
  {
    public const int k_iCallback = 1112;
    public ulong m_nGameID;
    public EResult m_eResult;
  }
}
