// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonObjectAttribute
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonObjectAttribute : JsonContainerAttribute
  {
    private MemberSerialization _memberSerialization;
    internal Required? _itemRequired;

    public MemberSerialization MemberSerialization
    {
      get => this._memberSerialization;
      set => this._memberSerialization = value;
    }

    public Required ItemRequired
    {
      get => this._itemRequired ?? Required.Default;
      set => this._itemRequired = new Required?(value);
    }

    public JsonObjectAttribute()
    {
    }

    public JsonObjectAttribute(MemberSerialization memberSerialization) => this.MemberSerialization = memberSerialization;

    public JsonObjectAttribute(string id)
      : base(id)
    {
    }
  }
}
