// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUserStats
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamUserStats
  {
    public static bool RequestCurrentStats()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_RequestCurrentStats();
    }

    public static bool GetStat(string pchName, out int pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetStat(pchName1, out pData);
    }

    public static bool GetStat(string pchName, out float pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetStat_(pchName1, out pData);
    }

    public static bool SetStat(string pchName, int nData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_SetStat(pchName1, nData);
    }

    public static bool SetStat(string pchName, float fData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_SetStat_(pchName1, fData);
    }

    public static bool UpdateAvgRateStat(
      string pchName,
      float flCountThisSession,
      double dSessionLength)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_UpdateAvgRateStat(pchName1, flCountThisSession, dSessionLength);
    }

    public static bool GetAchievement(string pchName, out bool pbAchieved)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetAchievement(pchName1, out pbAchieved);
    }

    public static bool SetAchievement(string pchName)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_SetAchievement(pchName1);
    }

    public static bool ClearAchievement(string pchName)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_ClearAchievement(pchName1);
    }

    public static bool GetAchievementAndUnlockTime(
      string pchName,
      out bool pbAchieved,
      out uint punUnlockTime)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(pchName1, out pbAchieved, out punUnlockTime);
    }

    public static bool StoreStats()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_StoreStats();
    }

    public static int GetAchievementIcon(string pchName)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetAchievementIcon(pchName1);
    }

    public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
      {
        using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
          return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(pchName1, pchKey1));
      }
    }

    public static bool IndicateAchievementProgress(
      string pchName,
      uint nCurProgress,
      uint nMaxProgress)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_IndicateAchievementProgress(pchName1, nCurProgress, nMaxProgress);
    }

    public static uint GetNumAchievements()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_GetNumAchievements();
    }

    public static string GetAchievementName(uint iAchievement)
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(iAchievement));
    }

    public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestUserStats(steamIDUser);
    }

    public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetUserStat(steamIDUser, pchName1, out pData);
    }

    public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetUserStat_(steamIDUser, pchName1, out pData);
    }

    public static bool GetUserAchievement(
      CSteamID steamIDUser,
      string pchName,
      out bool pbAchieved)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetUserAchievement(steamIDUser, pchName1, out pbAchieved);
    }

    public static bool GetUserAchievementAndUnlockTime(
      CSteamID steamIDUser,
      string pchName,
      out bool pbAchieved,
      out uint punUnlockTime)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(steamIDUser, pchName1, out pbAchieved, out punUnlockTime);
    }

    public static bool ResetAllStats(bool bAchievementsToo)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_ResetAllStats(bAchievementsToo);
    }

    public static SteamAPICall_t FindOrCreateLeaderboard(
      string pchLeaderboardName,
      ELeaderboardSortMethod eLeaderboardSortMethod,
      ELeaderboardDisplayType eLeaderboardDisplayType)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchLeaderboardName1 = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
        return (SteamAPICall_t) NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(pchLeaderboardName1, eLeaderboardSortMethod, eLeaderboardDisplayType);
    }

    public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchLeaderboardName1 = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
        return (SteamAPICall_t) NativeMethods.ISteamUserStats_FindLeaderboard(pchLeaderboardName1);
    }

    public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(hSteamLeaderboard));
    }

    public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(hSteamLeaderboard);
    }

    public static ELeaderboardSortMethod GetLeaderboardSortMethod(
      SteamLeaderboard_t hSteamLeaderboard)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(hSteamLeaderboard);
    }

    public static ELeaderboardDisplayType GetLeaderboardDisplayType(
      SteamLeaderboard_t hSteamLeaderboard)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(hSteamLeaderboard);
    }

    public static SteamAPICall_t DownloadLeaderboardEntries(
      SteamLeaderboard_t hSteamLeaderboard,
      ELeaderboardDataRequest eLeaderboardDataRequest,
      int nRangeStart,
      int nRangeEnd)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
    }

    public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(
      SteamLeaderboard_t hSteamLeaderboard,
      CSteamID[] prgUsers,
      int cUsers)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(hSteamLeaderboard, prgUsers, cUsers);
    }

    public static bool GetDownloadedLeaderboardEntry(
      SteamLeaderboardEntries_t hSteamLeaderboardEntries,
      int index,
      out LeaderboardEntry_t pLeaderboardEntry,
      int[] pDetails,
      int cDetailsMax)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
    }

    public static SteamAPICall_t UploadLeaderboardScore(
      SteamLeaderboard_t hSteamLeaderboard,
      ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod,
      int nScore,
      int[] pScoreDetails,
      int cScoreDetailsCount)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_UploadLeaderboardScore(hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
    }

    public static SteamAPICall_t AttachLeaderboardUGC(
      SteamLeaderboard_t hSteamLeaderboard,
      UGCHandle_t hUGC)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_AttachLeaderboardUGC(hSteamLeaderboard, hUGC);
    }

    public static SteamAPICall_t GetNumberOfCurrentPlayers()
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers();
    }

    public static SteamAPICall_t RequestGlobalAchievementPercentages()
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages();
    }

    public static int GetMostAchievedAchievementInfo(
      out string pchName,
      uint unNameBufLen,
      out float pflPercent,
      out bool pbAchieved)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal((int) unNameBufLen);
      int achievedAchievementInfo = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(num, unNameBufLen, out pflPercent, out pbAchieved);
      pchName = achievedAchievementInfo != -1 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return achievedAchievementInfo;
    }

    public static int GetNextMostAchievedAchievementInfo(
      int iIteratorPrevious,
      out string pchName,
      uint unNameBufLen,
      out float pflPercent,
      out bool pbAchieved)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal((int) unNameBufLen);
      int achievedAchievementInfo = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(iIteratorPrevious, num, unNameBufLen, out pflPercent, out pbAchieved);
      pchName = achievedAchievementInfo != -1 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return achievedAchievementInfo;
    }

    public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(pchName1, out pflPercent);
    }

    public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestGlobalStats(nHistoryDays);
    }

    public static bool GetGlobalStat(string pchStatName, out long pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchStatName1 = new InteropHelp.UTF8StringHandle(pchStatName))
        return NativeMethods.ISteamUserStats_GetGlobalStat(pchStatName1, out pData);
    }

    public static bool GetGlobalStat(string pchStatName, out double pData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchStatName1 = new InteropHelp.UTF8StringHandle(pchStatName))
        return NativeMethods.ISteamUserStats_GetGlobalStat_(pchStatName1, out pData);
    }

    public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchStatName1 = new InteropHelp.UTF8StringHandle(pchStatName))
        return NativeMethods.ISteamUserStats_GetGlobalStatHistory(pchStatName1, pData, cubData);
    }

    public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchStatName1 = new InteropHelp.UTF8StringHandle(pchStatName))
        return NativeMethods.ISteamUserStats_GetGlobalStatHistory_(pchStatName1, pData, cubData);
    }
  }
}
