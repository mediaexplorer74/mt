// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCUpdateHandle_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
  {
    public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);
    public ulong m_UGCUpdateHandle;

    public UGCUpdateHandle_t(ulong value) => this.m_UGCUpdateHandle = value;

    public override string ToString() => this.m_UGCUpdateHandle.ToString();

    public override bool Equals(object other) => other is UGCUpdateHandle_t ugcUpdateHandleT && this == ugcUpdateHandleT;

    public override int GetHashCode() => this.m_UGCUpdateHandle.GetHashCode();

    public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => (long) x.m_UGCUpdateHandle == (long) y.m_UGCUpdateHandle;

    public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => !(x == y);

    public static explicit operator UGCUpdateHandle_t(ulong value) => new UGCUpdateHandle_t(value);

    public static explicit operator ulong(UGCUpdateHandle_t that) => that.m_UGCUpdateHandle;

    public bool Equals(UGCUpdateHandle_t other) => (long) this.m_UGCUpdateHandle == (long) other.m_UGCUpdateHandle;

    public int CompareTo(UGCUpdateHandle_t other) => this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
  }
}
