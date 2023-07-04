// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonSerializationException
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonSerializationException : JsonException
  {
    public JsonSerializationException()
    {
    }

    public JsonSerializationException(string message)
      : base(message)
    {
    }

    public JsonSerializationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonSerializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static JsonSerializationException Create(JsonReader reader, string message) => JsonSerializationException.Create(reader, message, (Exception) null);

    internal static JsonSerializationException Create(
      JsonReader reader,
      string message,
      Exception ex)
    {
      return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
    }

    internal static JsonSerializationException Create(
      IJsonLineInfo lineInfo,
      string path,
      string message,
      Exception ex)
    {
      message = JsonPosition.FormatMessage(lineInfo, path, message);
      return new JsonSerializationException(message, ex);
    }
  }
}
