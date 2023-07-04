// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.NamingStrategy
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Serialization
{
  public abstract class NamingStrategy
  {
    public bool ProcessDictionaryKeys { get; set; }

    public bool ProcessExtensionDataNames { get; set; }

    public bool OverrideSpecifiedNames { get; set; }

    public virtual string GetPropertyName(string name, bool hasSpecifiedName) => hasSpecifiedName && !this.OverrideSpecifiedNames ? name : this.ResolvePropertyName(name);

    public virtual string GetExtensionDataName(string name) => !this.ProcessExtensionDataNames ? name : this.ResolvePropertyName(name);

    public virtual string GetDictionaryKey(string key) => !this.ProcessDictionaryKeys ? key : this.ResolvePropertyName(key);

    protected abstract string ResolvePropertyName(string name);
  }
}
