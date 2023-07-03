// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionState_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct P2PSessionState_t
  {
    public byte m_bConnectionActive;
    public byte m_bConnecting;
    public byte m_eP2PSessionError;
    public byte m_bUsingRelay;
    public int m_nBytesQueuedForSend;
    public int m_nPacketsQueuedForSend;
    public uint m_nRemoteIP;
    public ushort m_nRemotePort;
  }
}
