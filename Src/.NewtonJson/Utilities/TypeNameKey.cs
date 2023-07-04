// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.TypeNameKey
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal struct TypeNameKey : IEquatable<TypeNameKey>
  {
    internal readonly string AssemblyName;
    internal readonly string TypeName;

    public TypeNameKey(string assemblyName, string typeName)
    {
      this.AssemblyName = assemblyName;
      this.TypeName = typeName;
    }

    public override int GetHashCode()
    {
      string assemblyName = this.AssemblyName;
      int hashCode1 = assemblyName != null ? assemblyName.GetHashCode() : 0;
      string typeName = this.TypeName;
      int hashCode2 = typeName != null ? typeName.GetHashCode() : 0;
      return hashCode1 ^ hashCode2;
    }

    public override bool Equals(object obj) => obj is TypeNameKey other && this.Equals(other);

    public bool Equals(TypeNameKey other) => this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
  }
}
