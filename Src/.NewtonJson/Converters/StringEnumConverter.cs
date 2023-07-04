// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.StringEnumConverter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class StringEnumConverter : JsonConverter
  {
    public bool CamelCaseText { get; set; }

    public bool AllowIntegerValues { get; set; }

    public StringEnumConverter() => this.AllowIntegerValues = true;

    public StringEnumConverter(bool camelCaseText)
      : this()
    {
      this.CamelCaseText = camelCaseText;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Enum @enum = (Enum) value;
        string enumText = @enum.ToString("G");
        if (char.IsNumber(enumText[0]) || enumText[0] == '-')
        {
          if (!this.AllowIntegerValues)
            throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, "Integer value {0} is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumText), (Exception) null);
          writer.WriteValue(value);
        }
        else
        {
          string enumName = EnumUtils.ToEnumName(@enum.GetType(), enumText, this.CamelCaseText);
          writer.WriteValue(enumName);
        }
      }
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      bool isNullable = ReflectionUtils.IsNullableType(objectType);
      Type type = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;
      try
      {
        if (reader.TokenType == JsonToken.String)
          return EnumUtils.ParseEnumName(reader.Value.ToString(), isNullable, !this.AllowIntegerValues, type);
        if (reader.TokenType == JsonToken.Integer)
        {
          if (!this.AllowIntegerValues)
            throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, reader.Value));
          return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
        }
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.FormatValueForPrint(reader.Value), (object) objectType), ex);
      }
      throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    public override bool CanConvert(Type objectType) => (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum();
  }
}
