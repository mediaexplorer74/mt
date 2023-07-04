// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCaseNamingStrategy
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
  public class CamelCaseNamingStrategy : NamingStrategy
  {
    public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
    {
      this.ProcessDictionaryKeys = processDictionaryKeys;
      this.OverrideSpecifiedNames = overrideSpecifiedNames;
    }

    public CamelCaseNamingStrategy(
      bool processDictionaryKeys,
      bool overrideSpecifiedNames,
      bool processExtensionDataNames)
      : this(processDictionaryKeys, overrideSpecifiedNames)
    {
      this.ProcessExtensionDataNames = processExtensionDataNames;
    }

    public CamelCaseNamingStrategy()
    {
    }

    protected override string ResolvePropertyName(string name) => StringUtils.ToCamelCase(name);
  }
}
