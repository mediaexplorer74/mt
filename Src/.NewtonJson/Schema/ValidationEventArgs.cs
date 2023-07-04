// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.ValidationEventArgs
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See http://www.newtonsoft.com/jsonschema for more details.")]
  public class ValidationEventArgs : EventArgs
  {
    private readonly JsonSchemaException _ex;

    internal ValidationEventArgs(JsonSchemaException ex)
    {
      ValidationUtils.ArgumentNotNull((object) ex, nameof (ex));
      this._ex = ex;
    }

    public JsonSchemaException Exception => this._ex;

    public string Path => this._ex.Path;

    public string Message => this._ex.Message;
  }
}
