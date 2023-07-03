// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamScreenshots
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class SteamScreenshots
  {
    public static ScreenshotHandle WriteScreenshot(
      byte[] pubRGB,
      uint cubRGB,
      int nWidth,
      int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      return (ScreenshotHandle) NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
    }

    public static ScreenshotHandle AddScreenshotToLibrary(
      string pchFilename,
      string pchThumbnailFilename,
      int nWidth,
      int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFilename1 = new InteropHelp.UTF8StringHandle(pchFilename))
      {
        using (InteropHelp.UTF8StringHandle pchThumbnailFilename1 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
          return (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(pchFilename1, pchThumbnailFilename1, nWidth, nHeight);
      }
    }

    public static void TriggerScreenshot()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_TriggerScreenshot();
    }

    public static void HookScreenshots(bool bHook)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
    }

    public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchLocation1 = new InteropHelp.UTF8StringHandle(pchLocation))
        return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, pchLocation1);
    }

    public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
    }

    public static bool TagPublishedFile(
      ScreenshotHandle hScreenshot,
      PublishedFileId_t unPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
    }

    public static bool IsScreenshotsHooked()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
    }

    public static ScreenshotHandle AddVRScreenshotToLibrary(
      EVRScreenshotType eType,
      string pchFilename,
      string pchVRFilename)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFilename1 = new InteropHelp.UTF8StringHandle(pchFilename))
      {
        using (InteropHelp.UTF8StringHandle pchVRFilename1 = new InteropHelp.UTF8StringHandle(pchVRFilename))
          return (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, pchFilename1, pchVRFilename1);
      }
    }
  }
}
