// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorEventArgs
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  public class ErrorEventArgs : EventArgs
  {
    public object CurrentObject { get; }

    public ErrorContext ErrorContext { get; }

    public ErrorEventArgs(object currentObject, ErrorContext errorContext)
    {
      this.CurrentObject = currentObject;
      this.ErrorContext = errorContext;
    }
  }
}
