// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageAppSyncStatusCheck_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1305)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageAppSyncStatusCheck_t
  {
    public const int k_iCallback = 1305;
    public AppId_t m_nAppID;
    public EResult m_eResult;
  }
}
