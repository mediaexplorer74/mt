﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.HAuthTicket
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
  {
    public static readonly HAuthTicket Invalid = new HAuthTicket(0U);
    public uint m_HAuthTicket;

    public HAuthTicket(uint value) => this.m_HAuthTicket = value;

    public override string ToString() => this.m_HAuthTicket.ToString();

    public override bool Equals(object other) => other is HAuthTicket hauthTicket && this == hauthTicket;

    public override int GetHashCode() => this.m_HAuthTicket.GetHashCode();

    public static bool operator ==(HAuthTicket x, HAuthTicket y) => (int) x.m_HAuthTicket == (int) y.m_HAuthTicket;

    public static bool operator !=(HAuthTicket x, HAuthTicket y) => !(x == y);

    public static explicit operator HAuthTicket(uint value) => new HAuthTicket(value);

    public static explicit operator uint(HAuthTicket that) => that.m_HAuthTicket;

    public bool Equals(HAuthTicket other) => (int) this.m_HAuthTicket == (int) other.m_HAuthTicket;

    public int CompareTo(HAuthTicket other) => this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
  }
}
