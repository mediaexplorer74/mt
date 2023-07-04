﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DiscriminatedUnionConverter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  public class DiscriminatedUnionConverter : JsonConverter
  {
    private const string CasePropertyName = "Case";
    private const string FieldsPropertyName = "Fields";
    private static readonly ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));
    private static readonly ThreadSafeStore<Type, Type> UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));

    private static Type CreateUnionTypeLookup(Type t) => (Type) FSharpUtils.GetUnionCaseInfoDeclaringType(((IEnumerable<object>) (object[]) FSharpUtils.GetUnionCases((object) null, new object[2]
    {
      (object) t,
      null
    })).First<object>());

    private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
    {
      DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union();
      union.TagReader = (FSharpFunction) FSharpUtils.PreComputeUnionTagReader((object) null, new object[2]
      {
        (object) t,
        null
      });
      union.Cases = new List<DiscriminatedUnionConverter.UnionCase>();
      MethodCall<object, object> getUnionCases = FSharpUtils.GetUnionCases;
      object[] objArray = new object[2]{ (object) t, null };
      foreach (object target in (object[]) getUnionCases((object) null, objArray))
        union.Cases.Add(new DiscriminatedUnionConverter.UnionCase()
        {
          Tag = (int) FSharpUtils.GetUnionCaseInfoTag(target),
          Name = (string) FSharpUtils.GetUnionCaseInfoName(target),
          Fields = (PropertyInfo[]) FSharpUtils.GetUnionCaseInfoFields(target, new object[0]),
          FieldReader = (FSharpFunction) FSharpUtils.PreComputeUnionReader((object) null, new object[2]
          {
            target,
            null
          }),
          Constructor = (FSharpFunction) FSharpUtils.PreComputeUnionConstructor((object) null, new object[2]
          {
            target,
            null
          })
        });
      return union;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      Type key = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
      DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(key);
      int tag = (int) union.TagReader.Invoke(value);
      DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single<DiscriminatedUnionConverter.UnionCase>((Func<DiscriminatedUnionConverter.UnionCase, bool>) (c => c.Tag == tag));
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Case") : "Case");
      writer.WriteValue(unionCase.Name);
      if (unionCase.Fields != null && unionCase.Fields.Length != 0)
      {
        object[] objArray = (object[]) unionCase.FieldReader.Invoke(value);
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Fields") : "Fields");
        writer.WriteStartArray();
        foreach (object obj in objArray)
          serializer.Serialize(writer, obj);
        writer.WriteEndArray();
      }
      writer.WriteEndObject();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      DiscriminatedUnionConverter.UnionCase unionCase = (DiscriminatedUnionConverter.UnionCase) null;
      string caseName = (string) null;
      JArray jarray = (JArray) null;
      reader.ReadAndAssert();
      while (reader.TokenType == JsonToken.PropertyName)
      {
        string a = reader.Value.ToString();
        if (string.Equals(a, "Case", StringComparison.OrdinalIgnoreCase))
        {
          reader.ReadAndAssert();
          DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
          caseName = reader.Value.ToString();
          unionCase = union.Cases.SingleOrDefault<DiscriminatedUnionConverter.UnionCase>((Func<DiscriminatedUnionConverter.UnionCase, bool>) (c => c.Name == caseName));
          if (unionCase == null)
            throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) caseName));
        }
        else
        {
          if (!string.Equals(a, "Fields", StringComparison.OrdinalIgnoreCase))
            throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) a));
          reader.ReadAndAssert();
          jarray = reader.TokenType == JsonToken.StartArray ? (JArray) JToken.ReadFrom(reader) : throw JsonSerializationException.Create(reader, "Union fields must been an array.");
        }
        reader.ReadAndAssert();
      }
      if (unionCase == null)
        throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) "Case"));
      object[] objArray1 = new object[unionCase.Fields.Length];
      if (unionCase.Fields.Length != 0 && jarray == null)
        throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) "Fields"));
      if (jarray != null)
      {
        if (unionCase.Fields.Length != jarray.Count)
          throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) caseName));
        for (int index = 0; index < jarray.Count; ++index)
        {
          JToken jtoken = jarray[index];
          PropertyInfo field = unionCase.Fields[index];
          objArray1[index] = jtoken.ToObject(field.PropertyType, serializer);
        }
      }
      object[] objArray2 = new object[1]
      {
        (object) objArray1
      };
      return unionCase.Constructor.Invoke(objArray2);
    }

    public override bool CanConvert(Type objectType)
    {
      if (typeof (IEnumerable).IsAssignableFrom(objectType))
        return false;
      object[] customAttributes = objectType.GetCustomAttributes(true);
      bool flag = false;
      foreach (object obj in customAttributes)
      {
        Type type = obj.GetType();
        if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
        {
          FSharpUtils.EnsureInitialized(type.Assembly());
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
      return (bool) FSharpUtils.IsUnion((object) null, new object[2]
      {
        (object) objectType,
        null
      });
    }

    internal class Union
    {
      public List<DiscriminatedUnionConverter.UnionCase> Cases;

      public FSharpFunction TagReader { get; set; }
    }

    internal class UnionCase
    {
      public int Tag;
      public string Name;
      public PropertyInfo[] Fields;
      public FSharpFunction FieldReader;
      public FSharpFunction Constructor;
    }
  }
}
