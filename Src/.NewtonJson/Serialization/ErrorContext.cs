// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorContext
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  public class ErrorContext
  {
    internal ErrorContext(object originalObject, object member, string path, Exception error)
    {
      this.OriginalObject = originalObject;
      this.Member = member;
      this.Error = error;
      this.Path = path;
    }

    internal bool Traced { get; set; }

    public Exception Error { get; }

    public object OriginalObject { get; }

    public object Member { get; }

    public string Path { get; }

    public bool Handled { get; set; }
  }
}
