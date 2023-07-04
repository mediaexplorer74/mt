// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumValue`1
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal class EnumValue<T> where T : struct
  {
    private readonly string _name;
    private readonly T _value;

    public string Name => this._name;

    public T Value => this._value;

    public EnumValue(string name, T value)
    {
      this._name = name;
      this._value = value;
    }
  }
}
