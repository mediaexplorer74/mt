// Decompiled with JetBrains decompiler
// Type: Steamworks.GameServer
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class GameServer
  {
    public static bool Init(
      uint unIP,
      ushort usSteamPort,
      ushort usGamePort,
      ushort usQueryPort,
      EServerMode eServerMode,
      string pchVersionString)
    {
      InteropHelp.TestIfPlatformSupported();
      using (InteropHelp.UTF8StringHandle pchVersionString1 = new InteropHelp.UTF8StringHandle(pchVersionString))
        return NativeMethods.SteamGameServer_Init(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, pchVersionString1);
    }

    public static void Shutdown()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamGameServer_Shutdown();
    }

    public static void RunCallbacks()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamGameServer_RunCallbacks();
    }

    public static void ReleaseCurrentThreadMemory()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
    }

    public static bool BSecure()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamGameServer_BSecure();
    }

    public static CSteamID GetSteamID()
    {
      InteropHelp.TestIfPlatformSupported();
      return (CSteamID) NativeMethods.SteamGameServer_GetSteamID();
    }

    public static HSteamPipe GetHSteamPipe()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamPipe) NativeMethods.SteamGameServer_GetHSteamPipe();
    }

    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.SteamGameServer_GetHSteamUser();
    }
  }
}
