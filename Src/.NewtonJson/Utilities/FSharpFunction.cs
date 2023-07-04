// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.FSharpFunction
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal class FSharpFunction
  {
    private readonly object _instance;
    private readonly MethodCall<object, object> _invoker;

    public FSharpFunction(object instance, MethodCall<object, object> invoker)
    {
      this._instance = instance;
      this._invoker = invoker;
    }

    public object Invoke(params object[] args) => this._invoker(this._instance, args);
  }
}
