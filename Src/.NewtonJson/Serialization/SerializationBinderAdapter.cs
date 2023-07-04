// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SerializationBinderAdapter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal class SerializationBinderAdapter : ISerializationBinder
  {
    public readonly SerializationBinder SerializationBinder;

    public SerializationBinderAdapter(SerializationBinder serializationBinder) => this.SerializationBinder = serializationBinder;

    public Type BindToType(string assemblyName, string typeName) => this.SerializationBinder.BindToType(assemblyName, typeName);

    public void BindToName(Type serializedType, out string assemblyName, out string typeName) => this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
  }
}
