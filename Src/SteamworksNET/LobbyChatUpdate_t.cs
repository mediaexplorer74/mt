// Decompiled with JetBrains decompiler
// Type: Steamworks.LobbyChatUpdate_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(506)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LobbyChatUpdate_t
  {
    public const int k_iCallback = 506;
    public ulong m_ulSteamIDLobby;
    public ulong m_ulSteamIDUserChanged;
    public ulong m_ulSteamIDMakingChange;
    public uint m_rgfChatMemberStateChange;
  }
}
