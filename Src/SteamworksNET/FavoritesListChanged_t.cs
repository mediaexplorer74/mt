// Decompiled with JetBrains decompiler
// Type: Steamworks.FavoritesListChanged_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct FavoritesListChanged_t
  {
    public const int k_iCallback = 502;
    public uint m_nIP;
    public uint m_nQueryPort;
    public uint m_nConnPort;
    public uint m_nAppID;
    public uint m_nFlags;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAdd;
    public AccountID_t m_unAccountId;
  }
}
