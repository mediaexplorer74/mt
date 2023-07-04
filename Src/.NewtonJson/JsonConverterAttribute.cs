// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    public Type ConverterType => this._converterType;

    public object[] ConverterParameters { get; }

    public JsonConverterAttribute(Type converterType) => this._converterType = !(converterType == (Type) null) ? converterType : throw new ArgumentNullException(nameof (converterType));

    public JsonConverterAttribute(Type converterType, params object[] converterParameters)
      : this(converterType)
    {
      this.ConverterParameters = converterParameters;
    }
  }
}
