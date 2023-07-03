// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementStored_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1103)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct UserAchievementStored_t
  {
    public const int k_iCallback = 1103;
    public ulong m_nGameID;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bGroupAchievement;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string m_rgchAchievementName;
    public uint m_nCurProgress;
    public uint m_nMaxProgress;
  }
}
