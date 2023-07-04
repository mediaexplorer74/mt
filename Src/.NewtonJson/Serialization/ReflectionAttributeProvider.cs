// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionAttributeProvider
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  public class ReflectionAttributeProvider : IAttributeProvider
  {
    private readonly object _attributeProvider;

    public ReflectionAttributeProvider(object attributeProvider)
    {
      ValidationUtils.ArgumentNotNull(attributeProvider, nameof (attributeProvider));
      this._attributeProvider = attributeProvider;
    }

    public IList<Attribute> GetAttributes(bool inherit) => (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, (Type) null, inherit);

    public IList<Attribute> GetAttributes(Type attributeType, bool inherit) => (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
  }
}
