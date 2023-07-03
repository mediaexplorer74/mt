// Decompiled with JetBrains decompiler
// Type: Steamworks.GameConnectedChatLeave_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(340)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct GameConnectedChatLeave_t
  {
    public const int k_iCallback = 340;
    public CSteamID m_steamIDClanChat;
    public CSteamID m_steamIDUser;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bKicked;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bDropped;
  }
}
