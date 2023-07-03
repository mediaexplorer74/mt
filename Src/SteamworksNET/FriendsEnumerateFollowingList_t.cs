// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsEnumerateFollowingList_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(346)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendsEnumerateFollowingList_t
  {
    public const int k_iCallback = 346;
    public EResult m_eResult;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public CSteamID[] m_rgSteamID;
    public int m_nResultsReturned;
    public int m_nTotalResultCount;
  }
}
