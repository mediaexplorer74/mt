// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonExtensionDataAttribute
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class JsonExtensionDataAttribute : Attribute
  {
    public bool WriteData { get; set; }

    public bool ReadData { get; set; }

    public JsonExtensionDataAttribute()
    {
      this.WriteData = true;
      this.ReadData = true;
    }
  }
}
