// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamApps
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamApps
  {
    public static bool BIsSubscribed()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsSubscribed();
    }

    public static bool BIsLowViolence()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsLowViolence();
    }

    public static bool BIsCybercafe()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsCybercafe();
    }

    public static bool BIsVACBanned()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsVACBanned();
    }

    public static string GetCurrentGameLanguage()
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage());
    }

    public static string GetAvailableGameLanguages()
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages());
    }

    public static bool BIsSubscribedApp(AppId_t appID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsSubscribedApp(appID);
    }

    public static bool BIsDlcInstalled(AppId_t appID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsDlcInstalled(appID);
    }

    public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(nAppID);
    }

    public static bool BIsSubscribedFromFreeWeekend()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend();
    }

    public static int GetDLCCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_GetDLCCount();
    }

    public static bool BGetDLCDataByIndex(
      int iDLC,
      out AppId_t pAppID,
      out bool pbAvailable,
      out string pchName,
      int cchNameBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
      bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(iDLC, out pAppID, out pbAvailable, num, cchNameBufferSize);
      pchName = flag ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return flag;
    }

    public static void InstallDLC(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamApps_InstallDLC(nAppID);
    }

    public static void UninstallDLC(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamApps_UninstallDLC(nAppID);
    }

    public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(nAppID);
    }

    public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchNameBufferSize);
      bool currentBetaName = NativeMethods.ISteamApps_GetCurrentBetaName(num, cchNameBufferSize);
      pchName = currentBetaName ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return currentBetaName;
    }

    public static bool MarkContentCorrupt(bool bMissingFilesOnly)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_MarkContentCorrupt(bMissingFilesOnly);
    }

    public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_GetInstalledDepots(appID, pvecDepots, cMaxDepots);
    }

    public static uint GetAppInstallDir(
      AppId_t appID,
      out string pchFolder,
      uint cchFolderBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal((int) cchFolderBufferSize);
      uint appInstallDir = NativeMethods.ISteamApps_GetAppInstallDir(appID, num, cchFolderBufferSize);
      pchFolder = appInstallDir != 0U ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return appInstallDir;
    }

    public static bool BIsAppInstalled(AppId_t appID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_BIsAppInstalled(appID);
    }

    public static CSteamID GetAppOwner()
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamApps_GetAppOwner();
    }

    public static string GetLaunchQueryParam(string pchKey)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
        return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(pchKey1));
    }

    public static bool GetDlcDownloadProgress(
      AppId_t nAppID,
      out ulong punBytesDownloaded,
      out ulong punBytesTotal)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_GetDlcDownloadProgress(nAppID, out punBytesDownloaded, out punBytesTotal);
    }

    public static int GetAppBuildId()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamApps_GetAppBuildId();
    }

    public static void RequestAllProofOfPurchaseKeys()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys();
    }

    public static SteamAPICall_t GetFileDetails(string pszFileName)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pszFileName1 = new InteropHelp.UTF8StringHandle(pszFileName))
        return (SteamAPICall_t) NativeMethods.ISteamApps_GetFileDetails(pszFileName1);
    }
  }
}
