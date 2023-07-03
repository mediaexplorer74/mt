// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamVideo
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public static class SteamVideo
  {
    public static void GetVideoURL(AppId_t unVideoAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
    }

    public static bool IsBroadcasting(out int pnNumViewers)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
    }
  }
}
