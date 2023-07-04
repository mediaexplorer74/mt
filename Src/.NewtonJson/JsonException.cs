// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonException
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonException : Exception
  {
    public JsonException()
    {
    }

    public JsonException(string message)
      : base(message)
    {
    }

    public JsonException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
    {
      message = JsonPosition.FormatMessage(lineInfo, path, message);
      return new JsonException(message);
    }
  }
}
