﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageGetPublishedFileDetailsResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1318)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageGetPublishedFileDetailsResult_t
  {
    public const int k_iCallback = 1318;
    public EResult m_eResult;
    public PublishedFileId_t m_nPublishedFileId;
    public AppId_t m_nCreatorAppID;
    public AppId_t m_nConsumerAppID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
    public string m_rgchTitle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
    public string m_rgchDescription;
    public UGCHandle_t m_hFile;
    public UGCHandle_t m_hPreviewFile;
    public ulong m_ulSteamIDOwner;
    public uint m_rtimeCreated;
    public uint m_rtimeUpdated;
    public ERemoteStoragePublishedFileVisibility m_eVisibility;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bBanned;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
    public string m_rgchTags;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bTagsTruncated;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_pchFileName;
    public int m_nFileSize;
    public int m_nPreviewFileSize;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchURL;
    public EWorkshopFileType m_eFileType;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAcceptedForUse;
  }
}
