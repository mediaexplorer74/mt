// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    public bool AllowNullItems
    {
      get => this._allowNullItems;
      set => this._allowNullItems = value;
    }

    public JsonArrayAttribute()
    {
    }

    public JsonArrayAttribute(bool allowNullItems) => this._allowNullItems = allowNullItems;

    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
