// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameServerUtils
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamGameServerUtils
  {
    public static uint GetSecondsSinceAppActive()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
    }

    public static uint GetSecondsSinceComputerActive()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
    }

    public static EUniverse GetConnectedUniverse()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
    }

    public static uint GetServerRealTime()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetServerRealTime();
    }

    public static string GetIPCountry()
    {
      InteropHelp.TestIfAvailableGameServer();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetIPCountry());
    }

    public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
    }

    public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
    }

    public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
    }

    public static byte GetCurrentBatteryPower()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
    }

    public static AppId_t GetAppID()
    {
      InteropHelp.TestIfAvailableGameServer();
      return (AppId_t) NativeMethods.ISteamGameServerUtils_GetAppID();
    }

    public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
    {
      InteropHelp.TestIfAvailableGameServer();
      NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
    }

    public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
    }

    public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
    }

    public static bool GetAPICallResult(
      SteamAPICall_t hSteamAPICall,
      IntPtr pCallback,
      int cubCallback,
      int iCallbackExpected,
      out bool pbFailed)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
    }

    public static uint GetIPCCallCount()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
    }

    public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
    {
      InteropHelp.TestIfAvailableGameServer();
      NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
    }

    public static bool IsOverlayEnabled()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
    }

    public static bool BOverlayNeedsPresent()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
    }

    public static SteamAPICall_t CheckFileSignature(string szFileName)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle szFileName1 = new InteropHelp.UTF8StringHandle(szFileName))
        return (SteamAPICall_t) NativeMethods.ISteamGameServerUtils_CheckFileSignature(szFileName1);
    }

    public static bool ShowGamepadTextInput(
      EGamepadTextInputMode eInputMode,
      EGamepadTextInputLineMode eLineInputMode,
      string pchDescription,
      uint unCharMax,
      string pchExistingText)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchDescription1 = new InteropHelp.UTF8StringHandle(pchDescription))
      {
        using (InteropHelp.UTF8StringHandle pchExistingText1 = new InteropHelp.UTF8StringHandle(pchExistingText))
          return NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, pchDescription1, unCharMax, pchExistingText1);
      }
    }

    public static uint GetEnteredGamepadTextLength()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
    }

    public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
    {
      InteropHelp.TestIfAvailableGameServer();
      IntPtr num = Marshal.AllocHGlobal((int) cchText);
      bool gamepadTextInput = NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(num, cchText);
      pchText = gamepadTextInput ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return gamepadTextInput;
    }

    public static string GetSteamUILanguage()
    {
      InteropHelp.TestIfAvailableGameServer();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
    }

    public static bool IsSteamRunningInVR()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
    }

    public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
    {
      InteropHelp.TestIfAvailableGameServer();
      NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
    }

    public static bool IsSteamInBigPictureMode()
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerUtils_IsSteamInBigPictureMode();
    }

    public static void StartVRDashboard()
    {
      InteropHelp.TestIfAvailableGameServer();
      NativeMethods.ISteamGameServerUtils_StartVRDashboard();
    }
  }
}
