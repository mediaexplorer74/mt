// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Utilities
{
  internal static class EnumUtils
  {
    private static readonly ThreadSafeStore<Type, BidirectionalDictionary<string, string>> EnumMemberNamesPerType = new ThreadSafeStore<Type, BidirectionalDictionary<string, string>>(new Func<Type, BidirectionalDictionary<string, string>>(EnumUtils.InitializeEnumType));

    private static BidirectionalDictionary<string, string> InitializeEnumType(Type type)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = new BidirectionalDictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal, (IEqualityComparer<string>) StringComparer.Ordinal);
      foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        string name = field.Name;
        string second = field.GetCustomAttributes(typeof (EnumMemberAttribute), true).Cast<EnumMemberAttribute>().Select<EnumMemberAttribute, string>((Func<EnumMemberAttribute, string>) (a => a.Value)).SingleOrDefault<string>() ?? field.Name;
        if (bidirectionalDictionary.TryGetBySecond(second, out string _))
          throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) second, (object) type.Name));
        bidirectionalDictionary.Set(name, second);
      }
      return bidirectionalDictionary;
    }

    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type type = typeof (T);
      if (!type.IsDefined(typeof (FlagsAttribute), false))
        throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong uint64 = Convert.ToUInt64((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      IList<EnumValue<ulong>> namesAndValues = EnumUtils.GetNamesAndValues<T>();
      IList<T> flagsValues = (IList<T>) new List<T>();
      foreach (EnumValue<ulong> enumValue in (IEnumerable<EnumValue<ulong>>) namesAndValues)
      {
        if (((long) uint64 & (long) enumValue.Value) == (long) enumValue.Value && enumValue.Value != 0UL)
          flagsValues.Add((T) Convert.ChangeType((object) enumValue.Value, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (flagsValues.Count == 0 && namesAndValues.SingleOrDefault<EnumValue<ulong>>((Func<EnumValue<ulong>, bool>) (v => v.Value == 0UL)) != null)
        flagsValues.Add(default (T));
      return flagsValues;
    }

    public static IList<EnumValue<ulong>> GetNamesAndValues<T>() where T : struct => EnumUtils.GetNamesAndValues<ulong>(typeof (T));

    public static IList<EnumValue<TUnderlyingType>> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException(nameof (enumType));
      IList<object> objectList = enumType.IsEnum() ? EnumUtils.GetValues(enumType) : throw new ArgumentException("Type {0} is not an enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType.Name), nameof (enumType));
      IList<string> names = EnumUtils.GetNames(enumType);
      IList<EnumValue<TUnderlyingType>> namesAndValues = (IList<EnumValue<TUnderlyingType>>) new List<EnumValue<TUnderlyingType>>();
      for (int index = 0; index < objectList.Count; ++index)
      {
        try
        {
          namesAndValues.Add(new EnumValue<TUnderlyingType>(names[index], (TUnderlyingType) Convert.ChangeType(objectList[index], typeof (TUnderlyingType), (IFormatProvider) CultureInfo.CurrentCulture)));
        }
        catch (OverflowException ex)
        {
          throw new InvalidOperationException("Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) Enum.GetUnderlyingType(enumType), (object) typeof (TUnderlyingType), (object) Convert.ToUInt64(objectList[index], (IFormatProvider) CultureInfo.InvariantCulture)), (Exception) ex);
        }
      }
      return namesAndValues;
    }

    public static IList<object> GetValues(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type {0} is not an enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType.Name), nameof (enumType));
      List<object> values = new List<object>();
      foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        object obj = field.GetValue((object) enumType);
        values.Add(obj);
      }
      return (IList<object>) values;
    }

    public static IList<string> GetNames(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type {0} is not an enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType.Name), nameof (enumType));
      List<string> names = new List<string>();
      foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
        names.Add(field.Name);
      return (IList<string>) names;
    }

    public static object ParseEnumName(
      string enumText,
      bool isNullable,
      bool disallowValue,
      Type t)
    {
      if (enumText == string.Empty & isNullable)
        return (object) null;
      BidirectionalDictionary<string, string> map = EnumUtils.EnumMemberNamesPerType.Get(t);
      string resolvedEnumName;
      string str;
      if (EnumUtils.TryResolvedEnumName(map, enumText, out resolvedEnumName))
        str = resolvedEnumName;
      else if (enumText.IndexOf(',') != -1)
      {
        string[] strArray = enumText.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          string enumText1 = strArray[index].Trim();
          strArray[index] = EnumUtils.TryResolvedEnumName(map, enumText1, out resolvedEnumName) ? resolvedEnumName : enumText1;
        }
        str = string.Join(", ", strArray);
      }
      else
      {
        str = enumText;
        if (disallowValue)
        {
          bool flag = true;
          for (int index = 0; index < str.Length; ++index)
          {
            if (!char.IsNumber(str[index]))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            throw new FormatException("Integer string '{0}' is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumText));
        }
      }
      return Enum.Parse(t, str, true);
    }

    public static string ToEnumName(Type enumType, string enumText, bool camelCaseText)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = EnumUtils.EnumMemberNamesPerType.Get(enumType);
      string[] strArray = enumText.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string first = strArray[index].Trim();
        string second;
        bidirectionalDictionary.TryGetByFirst(first, out second);
        second = second ?? first;
        if (camelCaseText)
          second = StringUtils.ToCamelCase(second);
        strArray[index] = second;
      }
      return string.Join(", ", strArray);
    }

    private static bool TryResolvedEnumName(
      BidirectionalDictionary<string, string> map,
      string enumText,
      out string resolvedEnumName)
    {
      if (map.TryGetBySecond(enumText, out resolvedEnumName))
        return true;
      resolvedEnumName = (string) null;
      return false;
    }
  }
}
