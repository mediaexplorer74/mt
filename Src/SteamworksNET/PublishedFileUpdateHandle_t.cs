// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileUpdateHandle_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct PublishedFileUpdateHandle_t : 
    IEquatable<PublishedFileUpdateHandle_t>,
    IComparable<PublishedFileUpdateHandle_t>
  {
    public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
    public ulong m_PublishedFileUpdateHandle;

    public PublishedFileUpdateHandle_t(ulong value) => this.m_PublishedFileUpdateHandle = value;

    public override string ToString() => this.m_PublishedFileUpdateHandle.ToString();

    public override bool Equals(object other) => other is PublishedFileUpdateHandle_t fileUpdateHandleT && this == fileUpdateHandleT;

    public override int GetHashCode() => this.m_PublishedFileUpdateHandle.GetHashCode();

    public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => (long) x.m_PublishedFileUpdateHandle == (long) y.m_PublishedFileUpdateHandle;

    public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => !(x == y);

    public static explicit operator PublishedFileUpdateHandle_t(ulong value) => new PublishedFileUpdateHandle_t(value);

    public static explicit operator ulong(PublishedFileUpdateHandle_t that) => that.m_PublishedFileUpdateHandle;

    public bool Equals(PublishedFileUpdateHandle_t other) => (long) this.m_PublishedFileUpdateHandle == (long) other.m_PublishedFileUpdateHandle;

    public int CompareTo(PublishedFileUpdateHandle_t other) => this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
  }
}
