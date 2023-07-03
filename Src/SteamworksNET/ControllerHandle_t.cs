// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerHandle_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ControllerHandle_t : IEquatable<ControllerHandle_t>, IComparable<ControllerHandle_t>
  {
    public ulong m_ControllerHandle;

    public ControllerHandle_t(ulong value) => this.m_ControllerHandle = value;

    public override string ToString() => this.m_ControllerHandle.ToString();

    public override bool Equals(object other) => other is ControllerHandle_t controllerHandleT && this == controllerHandleT;

    public override int GetHashCode() => this.m_ControllerHandle.GetHashCode();

    public static bool operator ==(ControllerHandle_t x, ControllerHandle_t y) => (long) x.m_ControllerHandle == (long) y.m_ControllerHandle;

    public static bool operator !=(ControllerHandle_t x, ControllerHandle_t y) => !(x == y);

    public static explicit operator ControllerHandle_t(ulong value) => new ControllerHandle_t(value);

    public static explicit operator ulong(ControllerHandle_t that) => that.m_ControllerHandle;

    public bool Equals(ControllerHandle_t other) => (long) this.m_ControllerHandle == (long) other.m_ControllerHandle;

    public int CompareTo(ControllerHandle_t other) => this.m_ControllerHandle.CompareTo(other.m_ControllerHandle);
  }
}
