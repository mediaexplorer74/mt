// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestHandle
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
  {
    public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);
    public uint m_HTTPRequestHandle;

    public HTTPRequestHandle(uint value) => this.m_HTTPRequestHandle = value;

    public override string ToString() => this.m_HTTPRequestHandle.ToString();

    public override bool Equals(object other) => other is HTTPRequestHandle httpRequestHandle && this == httpRequestHandle;

    public override int GetHashCode() => this.m_HTTPRequestHandle.GetHashCode();

    public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y) => (int) x.m_HTTPRequestHandle == (int) y.m_HTTPRequestHandle;

    public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y) => !(x == y);

    public static explicit operator HTTPRequestHandle(uint value) => new HTTPRequestHandle(value);

    public static explicit operator uint(HTTPRequestHandle that) => that.m_HTTPRequestHandle;

    public bool Equals(HTTPRequestHandle other) => (int) this.m_HTTPRequestHandle == (int) other.m_HTTPRequestHandle;

    public int CompareTo(HTTPRequestHandle other) => this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
  }
}
