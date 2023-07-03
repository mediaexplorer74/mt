// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class SteamAPI
  {
    public static bool InitSafe() => SteamAPI.Init();

    public static bool Init()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_Init();
    }

    public static void Shutdown()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_Shutdown();
    }

    public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
    }

    public static void ReleaseCurrentThreadMemory()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
    }

    public static void RunCallbacks()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_RunCallbacks();
    }

    public static bool IsSteamRunning()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_IsSteamRunning();
    }

    public static HSteamUser GetHSteamUserCurrent()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.Steam_GetHSteamUserCurrent();
    }

    public static HSteamPipe GetHSteamPipe()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamPipe) NativeMethods.SteamAPI_GetHSteamPipe();
    }

    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.SteamAPI_GetHSteamUser();
    }
  }
}
