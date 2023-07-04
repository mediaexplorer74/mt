// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
  public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
  {
    private static readonly object TypeContractCacheLock = new object();
    private static readonly PropertyNameTable NameTable = new PropertyNameTable();
    private static Dictionary<ResolverContractKey, JsonContract> _contractCache;

    public CamelCasePropertyNamesContractResolver()
    {
      CamelCaseNamingStrategy caseNamingStrategy = new CamelCaseNamingStrategy();
      caseNamingStrategy.ProcessDictionaryKeys = true;
      caseNamingStrategy.OverrideSpecifiedNames = true;
      this.NamingStrategy = (NamingStrategy) caseNamingStrategy;
    }

    public override JsonContract ResolveContract(Type type)
    {
      ResolverContractKey key = !(type == (Type) null) ? new ResolverContractKey(this.GetType(), type) : throw new ArgumentNullException(nameof (type));
      Dictionary<ResolverContractKey, JsonContract> contractCache1 = CamelCasePropertyNamesContractResolver._contractCache;
      JsonContract contract;
      if (contractCache1 == null || !contractCache1.TryGetValue(key, out contract))
      {
        contract = this.CreateContract(type);
        lock (CamelCasePropertyNamesContractResolver.TypeContractCacheLock)
        {
          Dictionary<ResolverContractKey, JsonContract> contractCache2 = CamelCasePropertyNamesContractResolver._contractCache;
          Dictionary<ResolverContractKey, JsonContract> dictionary = contractCache2 != null ? new Dictionary<ResolverContractKey, JsonContract>((IDictionary<ResolverContractKey, JsonContract>) contractCache2) : new Dictionary<ResolverContractKey, JsonContract>();
          dictionary[key] = contract;
          CamelCasePropertyNamesContractResolver._contractCache = dictionary;
        }
      }
      return contract;
    }

    internal override PropertyNameTable GetNameTable() => CamelCasePropertyNamesContractResolver.NameTable;
  }
}
