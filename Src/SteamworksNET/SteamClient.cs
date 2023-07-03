﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamClient
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  public static class SteamClient
  {
    public static HSteamPipe CreateSteamPipe()
    {
      InteropHelp.TestIfAvailableClient();
      return (HSteamPipe) NativeMethods.ISteamClient_CreateSteamPipe();
    }

    public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamClient_BReleaseSteamPipe(hSteamPipe);
    }

    public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
    {
      InteropHelp.TestIfAvailableClient();
      return (HSteamUser) NativeMethods.ISteamClient_ConnectToGlobalUser(hSteamPipe);
    }

    public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
    {
      InteropHelp.TestIfAvailableClient();
      return (HSteamUser) NativeMethods.ISteamClient_CreateLocalUser(out phSteamPipe, eAccountType);
    }

    public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamClient_ReleaseUser(hSteamPipe, hUser);
    }

    public static IntPtr GetISteamUser(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamUser(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamGameServer(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamGameServer(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static void SetLocalIPBinding(uint unIP, ushort usPort)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamClient_SetLocalIPBinding(unIP, usPort);
    }

    public static IntPtr GetISteamFriends(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamFriends(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamUtils(hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamMatchmaking(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamMatchmaking(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamMatchmakingServers(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamMatchmakingServers(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamGenericInterface(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamGenericInterface(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamUserStats(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamUserStats(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamGameServerStats(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamGameServerStats(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamApps(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamApps(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamNetworking(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamNetworking(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamRemoteStorage(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamRemoteStorage(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamScreenshots(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamScreenshots(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static uint GetIPCCallCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamClient_GetIPCCallCount();
    }

    public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamClient_SetWarningMessageHook(pFunction);
    }

    public static bool BShutdownIfAllPipesClosed()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed();
    }

    public static IntPtr GetISteamHTTP(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamHTTP(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamUnifiedMessages(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamUnifiedMessages(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamController(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamController(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamUGC(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamUGC(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamAppList(
      HSteamUser hSteamUser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamAppList(hSteamUser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamMusic(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamMusic(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamMusicRemote(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamMusicRemote(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamHTMLSurface(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamHTMLSurface(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamInventory(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamInventory(hSteamuser, hSteamPipe, pchVersion1);
    }

    public static IntPtr GetISteamVideo(
      HSteamUser hSteamuser,
      HSteamPipe hSteamPipe,
      string pchVersion)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchVersion1 = new InteropHelp.UTF8StringHandle(pchVersion))
        return NativeMethods.ISteamClient_GetISteamVideo(hSteamuser, hSteamPipe, pchVersion1);
    }
  }
}
