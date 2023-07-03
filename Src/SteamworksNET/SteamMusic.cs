// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMusic
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class SteamMusic
  {
    public static bool BIsEnabled()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsEnabled();
    }

    public static bool BIsPlaying()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsPlaying();
    }

    public static AudioPlayback_Status GetPlaybackStatus()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetPlaybackStatus();
    }

    public static void Play()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Play();
    }

    public static void Pause()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Pause();
    }

    public static void PlayPrevious()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayPrevious();
    }

    public static void PlayNext()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayNext();
    }

    public static void SetVolume(float flVolume)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_SetVolume(flVolume);
    }

    public static float GetVolume()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetVolume();
    }
  }
}
