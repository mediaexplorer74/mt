// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCHandle_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
  {
    public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);
    public ulong m_UGCHandle;

    public UGCHandle_t(ulong value) => this.m_UGCHandle = value;

    public override string ToString() => this.m_UGCHandle.ToString();

    public override bool Equals(object other) => other is UGCHandle_t ugcHandleT && this == ugcHandleT;

    public override int GetHashCode() => this.m_UGCHandle.GetHashCode();

    public static bool operator ==(UGCHandle_t x, UGCHandle_t y) => (long) x.m_UGCHandle == (long) y.m_UGCHandle;

    public static bool operator !=(UGCHandle_t x, UGCHandle_t y) => !(x == y);

    public static explicit operator UGCHandle_t(ulong value) => new UGCHandle_t(value);

    public static explicit operator ulong(UGCHandle_t that) => that.m_UGCHandle;

    public bool Equals(UGCHandle_t other) => (long) this.m_UGCHandle == (long) other.m_UGCHandle;

    public int CompareTo(UGCHandle_t other) => this.m_UGCHandle.CompareTo(other.m_UGCHandle);
  }
}
