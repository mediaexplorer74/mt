﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetSocket_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
  {
    public uint m_SNetSocket;

    public SNetSocket_t(uint value) => this.m_SNetSocket = value;

    public override string ToString() => this.m_SNetSocket.ToString();

    public override bool Equals(object other) => other is SNetSocket_t snetSocketT && this == snetSocketT;

    public override int GetHashCode() => this.m_SNetSocket.GetHashCode();

    public static bool operator ==(SNetSocket_t x, SNetSocket_t y) => (int) x.m_SNetSocket == (int) y.m_SNetSocket;

    public static bool operator !=(SNetSocket_t x, SNetSocket_t y) => !(x == y);

    public static explicit operator SNetSocket_t(uint value) => new SNetSocket_t(value);

    public static explicit operator uint(SNetSocket_t that) => that.m_SNetSocket;

    public bool Equals(SNetSocket_t other) => (int) this.m_SNetSocket == (int) other.m_SNetSocket;

    public int CompareTo(SNetSocket_t other) => this.m_SNetSocket.CompareTo(other.m_SNetSocket);
  }
}
