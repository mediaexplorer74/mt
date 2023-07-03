﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMatchmaking
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamMatchmaking
  {
    public static int GetFavoriteGameCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetFavoriteGameCount();
    }

    public static bool GetFavoriteGame(
      int iGame,
      out AppId_t pnAppID,
      out uint pnIP,
      out ushort pnConnPort,
      out ushort pnQueryPort,
      out uint punFlags,
      out uint pRTime32LastPlayedOnServer)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
    }

    public static int AddFavoriteGame(
      AppId_t nAppID,
      uint nIP,
      ushort nConnPort,
      ushort nQueryPort,
      uint unFlags,
      uint rTime32LastPlayedOnServer)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_AddFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags, rTime32LastPlayedOnServer);
    }

    public static bool RemoveFavoriteGame(
      AppId_t nAppID,
      uint nIP,
      ushort nConnPort,
      ushort nQueryPort,
      uint unFlags)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_RemoveFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags);
    }

    public static SteamAPICall_t RequestLobbyList()
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_RequestLobbyList();
    }

    public static void AddRequestLobbyListStringFilter(
      string pchKeyToMatch,
      string pchValueToMatch,
      ELobbyComparison eComparisonType)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKeyToMatch1 = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
      {
        using (InteropHelp.UTF8StringHandle pchValueToMatch1 = new InteropHelp.UTF8StringHandle(pchValueToMatch))
          NativeMethods.ISteamMatchmaking_AddRequestLobbyListStringFilter(pchKeyToMatch1, pchValueToMatch1, eComparisonType);
      }
    }

    public static void AddRequestLobbyListNumericalFilter(
      string pchKeyToMatch,
      int nValueToMatch,
      ELobbyComparison eComparisonType)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKeyToMatch1 = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
        NativeMethods.ISteamMatchmaking_AddRequestLobbyListNumericalFilter(pchKeyToMatch1, nValueToMatch, eComparisonType);
    }

    public static void AddRequestLobbyListNearValueFilter(
      string pchKeyToMatch,
      int nValueToBeCloseTo)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKeyToMatch1 = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
        NativeMethods.ISteamMatchmaking_AddRequestLobbyListNearValueFilter(pchKeyToMatch1, nValueToBeCloseTo);
    }

    public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);
    }

    public static void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);
    }

    public static void AddRequestLobbyListResultCountFilter(int cMaxResults)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_AddRequestLobbyListResultCountFilter(cMaxResults);
    }

    public static void AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);
    }

    public static CSteamID GetLobbyByIndex(int iLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyByIndex(iLobby);
    }

    public static SteamAPICall_t CreateLobby(ELobbyType eLobbyType, int cMaxMembers)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_CreateLobby(eLobbyType, cMaxMembers);
    }

    public static SteamAPICall_t JoinLobby(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_JoinLobby(steamIDLobby);
    }

    public static void LeaveLobby(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_LeaveLobby(steamIDLobby);
    }

    public static bool InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_InviteUserToLobby(steamIDLobby, steamIDInvitee);
    }

    public static int GetNumLobbyMembers(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetNumLobbyMembers(steamIDLobby);
    }

    public static CSteamID GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember)
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyMemberByIndex(steamIDLobby, iMember);
    }

    public static string GetLobbyData(CSteamID steamIDLobby, string pchKey)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
        return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyData(steamIDLobby, pchKey1));
    }

    public static bool SetLobbyData(CSteamID steamIDLobby, string pchKey, string pchValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      {
        using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
          return NativeMethods.ISteamMatchmaking_SetLobbyData(steamIDLobby, pchKey1, pchValue1);
      }
    }

    public static int GetLobbyDataCount(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetLobbyDataCount(steamIDLobby);
    }

    public static bool GetLobbyDataByIndex(
      CSteamID steamIDLobby,
      int iLobbyData,
      out string pchKey,
      int cchKeyBufferSize,
      out string pchValue,
      int cchValueBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num1 = Marshal.AllocHGlobal(cchKeyBufferSize);
      IntPtr num2 = Marshal.AllocHGlobal(cchValueBufferSize);
      bool lobbyDataByIndex = NativeMethods.ISteamMatchmaking_GetLobbyDataByIndex(steamIDLobby, iLobbyData, num1, cchKeyBufferSize, num2, cchValueBufferSize);
      pchKey = lobbyDataByIndex ? InteropHelp.PtrToStringUTF8(num1) : (string) null;
      Marshal.FreeHGlobal(num1);
      pchValue = lobbyDataByIndex ? InteropHelp.PtrToStringUTF8(num2) : (string) null;
      Marshal.FreeHGlobal(num2);
      return lobbyDataByIndex;
    }

    public static bool DeleteLobbyData(CSteamID steamIDLobby, string pchKey)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
        return NativeMethods.ISteamMatchmaking_DeleteLobbyData(steamIDLobby, pchKey1);
    }

    public static string GetLobbyMemberData(
      CSteamID steamIDLobby,
      CSteamID steamIDUser,
      string pchKey)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
        return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyMemberData(steamIDLobby, steamIDUser, pchKey1));
    }

    public static void SetLobbyMemberData(CSteamID steamIDLobby, string pchKey, string pchValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      {
        using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
          NativeMethods.ISteamMatchmaking_SetLobbyMemberData(steamIDLobby, pchKey1, pchValue1);
      }
    }

    public static bool SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);
    }

    public static int GetLobbyChatEntry(
      CSteamID steamIDLobby,
      int iChatID,
      out CSteamID pSteamIDUser,
      byte[] pvData,
      int cubData,
      out EChatEntryType peChatEntryType)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData, out peChatEntryType);
    }

    public static bool RequestLobbyData(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_RequestLobbyData(steamIDLobby);
    }

    public static void SetLobbyGameServer(
      CSteamID steamIDLobby,
      uint unGameServerIP,
      ushort unGameServerPort,
      CSteamID steamIDGameServer)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmaking_SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);
    }

    public static bool GetLobbyGameServer(
      CSteamID steamIDLobby,
      out uint punGameServerIP,
      out ushort punGameServerPort,
      out CSteamID psteamIDGameServer)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
    }

    public static bool SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SetLobbyMemberLimit(steamIDLobby, cMaxMembers);
    }

    public static int GetLobbyMemberLimit(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_GetLobbyMemberLimit(steamIDLobby);
    }

    public static bool SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SetLobbyType(steamIDLobby, eLobbyType);
    }

    public static bool SetLobbyJoinable(CSteamID steamIDLobby, bool bLobbyJoinable)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SetLobbyJoinable(steamIDLobby, bLobbyJoinable);
    }

    public static CSteamID GetLobbyOwner(CSteamID steamIDLobby)
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyOwner(steamIDLobby);
    }

    public static bool SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SetLobbyOwner(steamIDLobby, steamIDNewOwner);
    }

    public static bool SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmaking_SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
    }
  }
}
