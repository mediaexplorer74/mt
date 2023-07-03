// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientUnifiedMessageHandle
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ClientUnifiedMessageHandle : 
    IEquatable<ClientUnifiedMessageHandle>,
    IComparable<ClientUnifiedMessageHandle>
  {
    public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);
    public ulong m_ClientUnifiedMessageHandle;

    public ClientUnifiedMessageHandle(ulong value) => this.m_ClientUnifiedMessageHandle = value;

    public override string ToString() => this.m_ClientUnifiedMessageHandle.ToString();

    public override bool Equals(object other) => other is ClientUnifiedMessageHandle unifiedMessageHandle && this == unifiedMessageHandle;

    public override int GetHashCode() => this.m_ClientUnifiedMessageHandle.GetHashCode();

    public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => (long) x.m_ClientUnifiedMessageHandle == (long) y.m_ClientUnifiedMessageHandle;

    public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => !(x == y);

    public static explicit operator ClientUnifiedMessageHandle(ulong value) => new ClientUnifiedMessageHandle(value);

    public static explicit operator ulong(ClientUnifiedMessageHandle that) => that.m_ClientUnifiedMessageHandle;

    public bool Equals(ClientUnifiedMessageHandle other) => (long) this.m_ClientUnifiedMessageHandle == (long) other.m_ClientUnifiedMessageHandle;

    public int CompareTo(ClientUnifiedMessageHandle other) => this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
  }
}
