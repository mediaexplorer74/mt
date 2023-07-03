// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementIconFetched_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1109)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct UserAchievementIconFetched_t
  {
    public const int k_iCallback = 1109;
    public CGameID m_nGameID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string m_rgchAchievementName;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAchieved;
    public int m_nIconHandle;
  }
}
