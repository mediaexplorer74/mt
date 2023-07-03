﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.HHTMLBrowser
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
  {
    public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);
    public uint m_HHTMLBrowser;

    public HHTMLBrowser(uint value) => this.m_HHTMLBrowser = value;

    public override string ToString() => this.m_HHTMLBrowser.ToString();

    public override bool Equals(object other) => other is HHTMLBrowser hhtmlBrowser && this == hhtmlBrowser;

    public override int GetHashCode() => this.m_HHTMLBrowser.GetHashCode();

    public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y) => (int) x.m_HHTMLBrowser == (int) y.m_HHTMLBrowser;

    public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => !(x == y);

    public static explicit operator HHTMLBrowser(uint value) => new HHTMLBrowser(value);

    public static explicit operator uint(HHTMLBrowser that) => that.m_HHTMLBrowser;

    public bool Equals(HHTMLBrowser other) => (int) this.m_HHTMLBrowser == (int) other.m_HHTMLBrowser;

    public int CompareTo(HHTMLBrowser other) => this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
  }
}
