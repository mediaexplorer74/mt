// Decompiled with JetBrains decompiler
// Type: Steamworks.MatchMakingKeyValuePair_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  public struct MatchMakingKeyValuePair_t
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szKey;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szValue;

    private MatchMakingKeyValuePair_t(string strKey, string strValue)
    {
      this.m_szKey = strKey;
      this.m_szValue = strValue;
    }
  }
}
