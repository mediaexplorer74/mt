// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerActionSetHandle_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Serializable]
  public struct ControllerActionSetHandle_t : 
    IEquatable<ControllerActionSetHandle_t>,
    IComparable<ControllerActionSetHandle_t>
  {
    public ulong m_ControllerActionSetHandle;

    public ControllerActionSetHandle_t(ulong value) => this.m_ControllerActionSetHandle = value;

    public override string ToString() => this.m_ControllerActionSetHandle.ToString();

    public override bool Equals(object other) => other is ControllerActionSetHandle_t actionSetHandleT && this == actionSetHandleT;

    public override int GetHashCode() => this.m_ControllerActionSetHandle.GetHashCode();

    public static bool operator ==(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y) => (long) x.m_ControllerActionSetHandle == (long) y.m_ControllerActionSetHandle;

    public static bool operator !=(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y) => !(x == y);

    public static explicit operator ControllerActionSetHandle_t(ulong value) => new ControllerActionSetHandle_t(value);

    public static explicit operator ulong(ControllerActionSetHandle_t that) => that.m_ControllerActionSetHandle;

    public bool Equals(ControllerActionSetHandle_t other) => (long) this.m_ControllerActionSetHandle == (long) other.m_ControllerActionSetHandle;

    public int CompareTo(ControllerActionSetHandle_t other) => this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
  }
}
