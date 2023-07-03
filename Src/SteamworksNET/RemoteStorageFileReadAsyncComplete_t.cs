// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageFileReadAsyncComplete_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1332)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageFileReadAsyncComplete_t
  {
    public const int k_iCallback = 1332;
    public SteamAPICall_t m_hFileReadAsync;
    public EResult m_eResult;
    public uint m_nOffset;
    public uint m_cubRead;
  }
}
