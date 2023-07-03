// Decompiled with JetBrains decompiler
// Type: Steamworks.ManifestId_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
  {
    public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);
    public ulong m_ManifestId;

    public ManifestId_t(ulong value) => this.m_ManifestId = value;

    public override string ToString() => this.m_ManifestId.ToString();

    public override bool Equals(object other) => other is ManifestId_t manifestIdT && this == manifestIdT;

    public override int GetHashCode() => this.m_ManifestId.GetHashCode();

    public static bool operator ==(ManifestId_t x, ManifestId_t y) => (long) x.m_ManifestId == (long) y.m_ManifestId;

    public static bool operator !=(ManifestId_t x, ManifestId_t y) => !(x == y);

    public static explicit operator ManifestId_t(ulong value) => new ManifestId_t(value);

    public static explicit operator ulong(ManifestId_t that) => that.m_ManifestId;

    public bool Equals(ManifestId_t other) => (long) this.m_ManifestId == (long) other.m_ManifestId;

    public int CompareTo(ManifestId_t other) => this.m_ManifestId.CompareTo(other.m_ManifestId);
  }
}
