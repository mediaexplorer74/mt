// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentityAttribute
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
  internal class CallbackIdentityAttribute : Attribute
  {
    public int Identity { get; set; }

    public CallbackIdentityAttribute(int callbackNum) => this.Identity = callbackNum;
  }
}
