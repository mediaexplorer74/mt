// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ResolverContractKey
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  internal struct ResolverContractKey : IEquatable<ResolverContractKey>
  {
    private readonly Type _resolverType;
    private readonly Type _contractType;

    public ResolverContractKey(Type resolverType, Type contractType)
    {
      this._resolverType = resolverType;
      this._contractType = contractType;
    }

    public override int GetHashCode() => this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();

    public override bool Equals(object obj) => obj is ResolverContractKey other && this.Equals(other);

    public bool Equals(ResolverContractKey other) => this._resolverType == other._resolverType && this._contractType == other._contractType;
  }
}
