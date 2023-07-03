// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStoragePublishedFileSubscribed_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1321)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStoragePublishedFileSubscribed_t
  {
    public const int k_iCallback = 1321;
    public PublishedFileId_t m_nPublishedFileId;
    public AppId_t m_nAppID;
  }
}
