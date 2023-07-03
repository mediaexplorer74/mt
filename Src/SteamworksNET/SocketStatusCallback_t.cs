// Decompiled with JetBrains decompiler
// Type: Steamworks.SocketStatusCallback_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1201)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SocketStatusCallback_t
  {
    public const int k_iCallback = 1201;
    public SNetSocket_t m_hSocket;
    public SNetListenSocket_t m_hListenSocket;
    public CSteamID m_steamIDRemote;
    public int m_eSNetSocketState;
  }
}
