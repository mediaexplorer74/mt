// Decompiled with JetBrains decompiler
// Type: Steamworks.AvatarImageLoaded_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(334)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct AvatarImageLoaded_t
  {
    public const int k_iCallback = 334;
    public CSteamID m_steamID;
    public int m_iImage;
    public int m_iWide;
    public int m_iTall;
  }
}
