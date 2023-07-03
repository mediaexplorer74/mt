// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamUser
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
  {
    public int m_HSteamUser;

    public HSteamUser(int value) => this.m_HSteamUser = value;

    public override string ToString() => this.m_HSteamUser.ToString();

    public override bool Equals(object other) => other is HSteamUser hsteamUser && this == hsteamUser;

    public override int GetHashCode() => this.m_HSteamUser.GetHashCode();

    public static bool operator ==(HSteamUser x, HSteamUser y) => x.m_HSteamUser == y.m_HSteamUser;

    public static bool operator !=(HSteamUser x, HSteamUser y) => !(x == y);

    public static explicit operator HSteamUser(int value) => new HSteamUser(value);

    public static explicit operator int(HSteamUser that) => that.m_HSteamUser;

    public bool Equals(HSteamUser other) => this.m_HSteamUser == other.m_HSteamUser;

    public int CompareTo(HSteamUser other) => this.m_HSteamUser.CompareTo(other.m_HSteamUser);
  }
}
