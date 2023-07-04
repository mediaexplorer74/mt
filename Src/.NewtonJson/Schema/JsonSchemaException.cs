// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaException
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See http://www.newtonsoft.com/jsonschema for more details.")]
  [Serializable]
  public class JsonSchemaException : JsonException
  {
    public int LineNumber { get; }

    public int LinePosition { get; }

    public string Path { get; }

    public JsonSchemaException()
    {
    }

    public JsonSchemaException(string message)
      : base(message)
    {
    }

    public JsonSchemaException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonSchemaException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal JsonSchemaException(
      string message,
      Exception innerException,
      string path,
      int lineNumber,
      int linePosition)
      : base(message, innerException)
    {
      this.Path = path;
      this.LineNumber = lineNumber;
      this.LinePosition = linePosition;
    }
  }
}
