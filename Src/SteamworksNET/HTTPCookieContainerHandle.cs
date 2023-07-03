// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPCookieContainerHandle
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HTTPCookieContainerHandle : 
    IEquatable<HTTPCookieContainerHandle>,
    IComparable<HTTPCookieContainerHandle>
  {
    public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);
    public uint m_HTTPCookieContainerHandle;

    public HTTPCookieContainerHandle(uint value) => this.m_HTTPCookieContainerHandle = value;

    public override string ToString() => this.m_HTTPCookieContainerHandle.ToString();

    public override bool Equals(object other) => other is HTTPCookieContainerHandle cookieContainerHandle && this == cookieContainerHandle;

    public override int GetHashCode() => this.m_HTTPCookieContainerHandle.GetHashCode();

    public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => (int) x.m_HTTPCookieContainerHandle == (int) y.m_HTTPCookieContainerHandle;

    public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => !(x == y);

    public static explicit operator HTTPCookieContainerHandle(uint value) => new HTTPCookieContainerHandle(value);

    public static explicit operator uint(HTTPCookieContainerHandle that) => that.m_HTTPCookieContainerHandle;

    public bool Equals(HTTPCookieContainerHandle other) => (int) this.m_HTTPCookieContainerHandle == (int) other.m_HTTPCookieContainerHandle;

    public int CompareTo(HTTPCookieContainerHandle other) => this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
  }
}
