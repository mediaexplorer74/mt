// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientGameServerDeny_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(113)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ClientGameServerDeny_t
  {
    public const int k_iCallback = 113;
    public uint m_uAppID;
    public uint m_unGameServerIP;
    public ushort m_usGameServerPort;
    public ushort m_bSecure;
    public uint m_uReason;
  }
}
