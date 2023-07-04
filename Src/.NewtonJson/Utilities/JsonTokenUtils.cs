// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.JsonTokenUtils
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal static class JsonTokenUtils
  {
    internal static bool IsEndToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
        case JsonToken.EndArray:
        case JsonToken.EndConstructor:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsStartToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.StartObject:
        case JsonToken.StartArray:
        case JsonToken.StartConstructor:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsPrimitiveToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          return true;
        default:
          return false;
      }
    }
  }
}
