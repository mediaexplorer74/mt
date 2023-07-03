// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsGroupID_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
  {
    public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t((short) -1);
    public short m_FriendsGroupID;

    public FriendsGroupID_t(short value) => this.m_FriendsGroupID = value;

    public override string ToString() => this.m_FriendsGroupID.ToString();

    public override bool Equals(object other) => other is FriendsGroupID_t friendsGroupIdT && this == friendsGroupIdT;

    public override int GetHashCode() => this.m_FriendsGroupID.GetHashCode();

    public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y) => (int) x.m_FriendsGroupID == (int) y.m_FriendsGroupID;

    public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y) => !(x == y);

    public static explicit operator FriendsGroupID_t(short value) => new FriendsGroupID_t(value);

    public static explicit operator short(FriendsGroupID_t that) => that.m_FriendsGroupID;

    public bool Equals(FriendsGroupID_t other) => (int) this.m_FriendsGroupID == (int) other.m_FriendsGroupID;

    public int CompareTo(FriendsGroupID_t other) => this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
  }
}
