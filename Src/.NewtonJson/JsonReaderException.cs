// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReaderException
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonReaderException : JsonException
  {
    public int LineNumber { get; }

    public int LinePosition { get; }

    public string Path { get; }

    public JsonReaderException()
    {
    }

    public JsonReaderException(string message)
      : base(message)
    {
    }

    public JsonReaderException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonReaderException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public JsonReaderException(
      string message,
      string path,
      int lineNumber,
      int linePosition,
      Exception innerException)
      : base(message, innerException)
    {
      this.Path = path;
      this.LineNumber = lineNumber;
      this.LinePosition = linePosition;
    }

    internal static JsonReaderException Create(JsonReader reader, string message) => JsonReaderException.Create(reader, message, (Exception) null);

    internal static JsonReaderException Create(JsonReader reader, string message, Exception ex) => JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);

    internal static JsonReaderException Create(
      IJsonLineInfo lineInfo,
      string path,
      string message,
      Exception ex)
    {
      message = JsonPosition.FormatMessage(lineInfo, path, message);
      int lineNumber;
      int linePosition;
      if (lineInfo != null && lineInfo.HasLineInfo())
      {
        lineNumber = lineInfo.LineNumber;
        linePosition = lineInfo.LinePosition;
      }
      else
      {
        lineNumber = 0;
        linePosition = 0;
      }
      return new JsonReaderException(message, path, lineNumber, linePosition, ex);
    }
  }
}
