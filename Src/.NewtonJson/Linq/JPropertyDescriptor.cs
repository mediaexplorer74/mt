// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JPropertyDescriptor
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
  public class JPropertyDescriptor : PropertyDescriptor
  {
    public JPropertyDescriptor(string name)
      : base(name, (Attribute[]) null)
    {
    }

    private static JObject CastInstance(object instance) => (JObject) instance;

    public override bool CanResetValue(object component) => false;

    public override object GetValue(object component) => (object) JPropertyDescriptor.CastInstance(component)[this.Name];

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
      if (!(value is JToken jtoken1))
        jtoken1 = (JToken) new JValue(value);
      JToken jtoken2 = jtoken1;
      JPropertyDescriptor.CastInstance(component)[this.Name] = jtoken2;
    }

    public override bool ShouldSerializeValue(object component) => false;

    public override Type ComponentType => typeof (JObject);

    public override bool IsReadOnly => false;

    public override Type PropertyType => typeof (object);

    protected override int NameHashCode => base.NameHashCode;
  }
}
