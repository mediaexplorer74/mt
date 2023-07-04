// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.JavaScriptDateTimeConverter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class JavaScriptDateTimeConverter : DateTimeConverterBase
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      long javaScriptTicks;
      switch (value)
      {
        case DateTime dateTime:
          javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime.ToUniversalTime());
          break;
        case DateTimeOffset dateTimeOffset:
          javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.ToUniversalTime().UtcDateTime);
          break;
        default:
          throw new JsonSerializationException("Expected date object value.");
      }
      writer.WriteStartConstructor("Date");
      writer.WriteValue(javaScriptTicks);
      writer.WriteEndConstructor();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
        throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
      reader.Read();
      DateTime dateTime = reader.TokenType == JsonToken.Integer ? DateTimeUtils.ConvertJavaScriptTicksToDateTime((long) reader.Value) : throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      reader.Read();
      if (reader.TokenType != JsonToken.EndConstructor)
        throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      return (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof (DateTimeOffset) ? (object) new DateTimeOffset(dateTime) : (object) dateTime;
    }
  }
}
