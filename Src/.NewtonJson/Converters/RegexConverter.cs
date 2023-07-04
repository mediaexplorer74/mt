// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.RegexConverter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
  public class RegexConverter : JsonConverter
  {
    private const string PatternName = "Pattern";
    private const string OptionsName = "Options";

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Regex regex = (Regex) value;
      if (writer is BsonWriter writer1)
        this.WriteBson(writer1, regex);
      else
        this.WriteJson(writer, regex, serializer);
    }

    private bool HasFlag(RegexOptions options, RegexOptions flag) => (options & flag) == flag;

    private void WriteBson(BsonWriter writer, Regex regex)
    {
      string str = (string) null;
      if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
        str += "i";
      if (this.HasFlag(regex.Options, RegexOptions.Multiline))
        str += "m";
      if (this.HasFlag(regex.Options, RegexOptions.Singleline))
        str += "s";
      string options = str + "u";
      if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
        options += "x";
      writer.WriteRegex(regex.ToString(), options);
    }

    private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
    {
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
      writer.WriteValue(regex.ToString());
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Options") : "Options");
      serializer.Serialize(writer, (object) regex.Options);
      writer.WriteEndObject();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      switch (reader.TokenType)
      {
        case JsonToken.StartObject:
          return (object) this.ReadRegexObject(reader, serializer);
        case JsonToken.String:
          return this.ReadRegexString(reader);
        case JsonToken.Null:
          return (object) null;
        default:
          throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
      }
    }

    private object ReadRegexString(JsonReader reader)
    {
      string str1 = (string) reader.Value;
      int num = str1.LastIndexOf('/');
      string pattern = str1.Substring(1, num - 1);
      string str2 = str1.Substring(num + 1);
      RegexOptions options = RegexOptions.None;
      foreach (char ch in str2)
      {
        switch (ch)
        {
          case 'i':
            options |= RegexOptions.IgnoreCase;
            break;
          case 'm':
            options |= RegexOptions.Multiline;
            break;
          case 's':
            options |= RegexOptions.Singleline;
            break;
          case 'x':
            options |= RegexOptions.ExplicitCapture;
            break;
        }
      }
      return (object) new Regex(pattern, options);
    }

    private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
    {
      string pattern = (string) null;
      RegexOptions? nullable = new RegexOptions?();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string a = reader.Value.ToString();
            if (!reader.Read())
              throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
            if (string.Equals(a, "Pattern", StringComparison.OrdinalIgnoreCase))
            {
              pattern = (string) reader.Value;
              continue;
            }
            if (string.Equals(a, "Options", StringComparison.OrdinalIgnoreCase))
            {
              nullable = new RegexOptions?(serializer.Deserialize<RegexOptions>(reader));
              continue;
            }
            reader.Skip();
            continue;
          case JsonToken.EndObject:
            return pattern != null ? new Regex(pattern, (RegexOptions) ((int) nullable ?? 0)) : throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
          default:
            continue;
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
    }

    public override bool CanConvert(Type objectType) => objectType.Name == "Regex" && this.IsRegex(objectType);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool IsRegex(Type objectType) => objectType == typeof (Regex);
  }
}
