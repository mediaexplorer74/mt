﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamRemoteStorage
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Collections.Generic;

namespace Steamworks
{
  public static class SteamRemoteStorage
  {
    public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FileWrite(pchFile1, pvData, cubData);
    }

    public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FileRead(pchFile1, pvData, cubDataToRead);
    }

    public static SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileWriteAsync(pchFile1, pvData, cubData);
    }

    public static SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileReadAsync(pchFile1, nOffset, cubToRead);
    }

    public static bool FileReadAsyncComplete(
      SteamAPICall_t hReadCall,
      byte[] pvBuffer,
      uint cubToRead)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(hReadCall, pvBuffer, cubToRead);
    }

    public static bool FileForget(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FileForget(pchFile1);
    }

    public static bool FileDelete(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FileDelete(pchFile1);
    }

    public static SteamAPICall_t FileShare(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileShare(pchFile1);
    }

    public static bool SetSyncPlatforms(
      string pchFile,
      ERemoteStoragePlatform eRemoteStoragePlatform)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(pchFile1, eRemoteStoragePlatform);
    }

    public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return (UGCFileWriteStreamHandle_t) NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(pchFile1);
    }

    public static bool FileWriteStreamWriteChunk(
      UGCFileWriteStreamHandle_t writeHandle,
      byte[] pvData,
      int cubData)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(writeHandle, pvData, cubData);
    }

    public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(writeHandle);
    }

    public static bool FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(writeHandle);
    }

    public static bool FileExists(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FileExists(pchFile1);
    }

    public static bool FilePersisted(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_FilePersisted(pchFile1);
    }

    public static int GetFileSize(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_GetFileSize(pchFile1);
    }

    public static long GetFileTimestamp(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_GetFileTimestamp(pchFile1);
    }

    public static ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(pchFile1);
    }

    public static int GetFileCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_GetFileCount();
    }

    public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(iFile, out pnFileSizeInBytes));
    }

    public static bool GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_GetQuota(out pnTotalBytes, out puAvailableBytes);
    }

    public static bool IsCloudEnabledForAccount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount();
    }

    public static bool IsCloudEnabledForApp()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp();
    }

    public static void SetCloudEnabledForApp(bool bEnabled)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(bEnabled);
    }

    public static SteamAPICall_t UGCDownload(UGCHandle_t hContent, uint unPriority)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UGCDownload(hContent, unPriority);
    }

    public static bool GetUGCDownloadProgress(
      UGCHandle_t hContent,
      out int pnBytesDownloaded,
      out int pnBytesExpected)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(hContent, out pnBytesDownloaded, out pnBytesExpected);
    }

    public static bool GetUGCDetails(
      UGCHandle_t hContent,
      out AppId_t pnAppID,
      out string ppchName,
      out int pnFileSizeInBytes,
      out CSteamID pSteamIDOwner)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr ppchName1;
      bool ugcDetails = NativeMethods.ISteamRemoteStorage_GetUGCDetails(hContent, out pnAppID, out ppchName1, out pnFileSizeInBytes, out pSteamIDOwner);
      ppchName = ugcDetails ? InteropHelp.PtrToStringUTF8(ppchName1) : (string) null;
      return ugcDetails;
    }

    public static int UGCRead(
      UGCHandle_t hContent,
      byte[] pvData,
      int cubDataToRead,
      uint cOffset,
      EUGCReadAction eAction)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_UGCRead(hContent, pvData, cubDataToRead, cOffset, eAction);
    }

    public static int GetCachedUGCCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_GetCachedUGCCount();
    }

    public static UGCHandle_t GetCachedUGCHandle(int iCachedContent)
    {
      InteropHelp.TestIfAvailableClient();
      return (UGCHandle_t) NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(iCachedContent);
    }

    public static SteamAPICall_t PublishWorkshopFile(
      string pchFile,
      string pchPreviewFile,
      AppId_t nConsumerAppId,
      string pchTitle,
      string pchDescription,
      ERemoteStoragePublishedFileVisibility eVisibility,
      IList<string> pTags,
      EWorkshopFileType eWorkshopFileType)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
      {
        using (InteropHelp.UTF8StringHandle pchPreviewFile1 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
        {
          using (InteropHelp.UTF8StringHandle pchTitle1 = new InteropHelp.UTF8StringHandle(pchTitle))
          {
            using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
              return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(pchFile1, pchPreviewFile1, nConsumerAppId, pchTitle1, pchDescription1, eVisibility, (IntPtr) new InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
          }
        }
      }
    }

    public static PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(
      PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (PublishedFileUpdateHandle_t) NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(unPublishedFileId);
    }

    public static bool UpdatePublishedFileFile(
      PublishedFileUpdateHandle_t updateHandle,
      string pchFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFile1 = new InteropHelp.UTF8StringHandle(pchFile))
        return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(updateHandle, pchFile1);
    }

    public static bool UpdatePublishedFilePreviewFile(
      PublishedFileUpdateHandle_t updateHandle,
      string pchPreviewFile)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchPreviewFile1 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
        return NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(updateHandle, pchPreviewFile1);
    }

    public static bool UpdatePublishedFileTitle(
      PublishedFileUpdateHandle_t updateHandle,
      string pchTitle)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchTitle1 = new InteropHelp.UTF8StringHandle(pchTitle))
        return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(updateHandle, pchTitle1);
    }

    public static bool UpdatePublishedFileDescription(
      PublishedFileUpdateHandle_t updateHandle,
      string pchDescription)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
        return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(updateHandle, pchDescription1);
    }

    public static bool UpdatePublishedFileVisibility(
      PublishedFileUpdateHandle_t updateHandle,
      ERemoteStoragePublishedFileVisibility eVisibility)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(updateHandle, eVisibility);
    }

    public static bool UpdatePublishedFileTags(
      PublishedFileUpdateHandle_t updateHandle,
      IList<string> pTags)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(updateHandle, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
    }

    public static SteamAPICall_t CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(updateHandle);
    }

    public static SteamAPICall_t GetPublishedFileDetails(
      PublishedFileId_t unPublishedFileId,
      uint unMaxSecondsOld)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(unPublishedFileId, unMaxSecondsOld);
    }

    public static SteamAPICall_t DeletePublishedFile(PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_DeletePublishedFile(unPublishedFileId);
    }

    public static SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(unStartIndex);
    }

    public static SteamAPICall_t SubscribePublishedFile(PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(unPublishedFileId);
    }

    public static SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(unStartIndex);
    }

    public static SteamAPICall_t UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(unPublishedFileId);
    }

    public static bool UpdatePublishedFileSetChangeDescription(
      PublishedFileUpdateHandle_t updateHandle,
      string pchChangeDescription)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchChangeDescription1 = new InteropHelp.UTF8StringHandle(pchChangeDescription))
        return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(updateHandle, pchChangeDescription1);
    }

    public static SteamAPICall_t GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(unPublishedFileId);
    }

    public static SteamAPICall_t UpdateUserPublishedItemVote(
      PublishedFileId_t unPublishedFileId,
      bool bVoteUp)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(unPublishedFileId, bVoteUp);
    }

    public static SteamAPICall_t GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(unPublishedFileId);
    }

    public static SteamAPICall_t EnumerateUserSharedWorkshopFiles(
      CSteamID steamId,
      uint unStartIndex,
      IList<string> pRequiredTags,
      IList<string> pExcludedTags)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(steamId, unStartIndex, (IntPtr) new InteropHelp.SteamParamStringArray(pRequiredTags), (IntPtr) new InteropHelp.SteamParamStringArray(pExcludedTags));
    }

    public static SteamAPICall_t PublishVideo(
      EWorkshopVideoProvider eVideoProvider,
      string pchVideoAccount,
      string pchVideoIdentifier,
      string pchPreviewFile,
      AppId_t nConsumerAppId,
      string pchTitle,
      string pchDescription,
      ERemoteStoragePublishedFileVisibility eVisibility,
      IList<string> pTags)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVideoAccount1 = new InteropHelp.UTF8StringHandle(pchVideoAccount))
      {
        using (InteropHelp.UTF8StringHandle pchVideoIdentifier1 = new InteropHelp.UTF8StringHandle(pchVideoIdentifier))
        {
          using (InteropHelp.UTF8StringHandle pchPreviewFile1 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
          {
            using (InteropHelp.UTF8StringHandle pchTitle1 = new InteropHelp.UTF8StringHandle(pchTitle))
            {
              using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_PublishVideo(eVideoProvider, pchVideoAccount1, pchVideoIdentifier1, pchPreviewFile1, nConsumerAppId, pchTitle1, pchDescription1, eVisibility, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
            }
          }
        }
      }
    }

    public static SteamAPICall_t SetUserPublishedFileAction(
      PublishedFileId_t unPublishedFileId,
      EWorkshopFileAction eAction)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(unPublishedFileId, eAction);
    }

    public static SteamAPICall_t EnumeratePublishedFilesByUserAction(
      EWorkshopFileAction eAction,
      uint unStartIndex)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(eAction, unStartIndex);
    }

    public static SteamAPICall_t EnumeratePublishedWorkshopFiles(
      EWorkshopEnumerationType eEnumerationType,
      uint unStartIndex,
      uint unCount,
      uint unDays,
      IList<string> pTags,
      IList<string> pUserTags)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(eEnumerationType, unStartIndex, unCount, unDays, (IntPtr) new InteropHelp.SteamParamStringArray(pTags), (IntPtr) new InteropHelp.SteamParamStringArray(pUserTags));
    }

    public static SteamAPICall_t UGCDownloadToLocation(
      UGCHandle_t hContent,
      string pchLocation,
      uint unPriority)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchLocation1 = new InteropHelp.UTF8StringHandle(pchLocation))
        return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(hContent, pchLocation1, unPriority);
    }
  }
}
