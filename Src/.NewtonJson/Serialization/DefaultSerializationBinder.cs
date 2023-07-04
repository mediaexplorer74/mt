// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultSerializationBinder
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class DefaultSerializationBinder : SerializationBinder, ISerializationBinder
  {
    internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
    private readonly ThreadSafeStore<TypeNameKey, Type> _typeCache;

    public DefaultSerializationBinder() => this._typeCache = new ThreadSafeStore<TypeNameKey, Type>(new Func<TypeNameKey, Type>(this.GetTypeFromTypeNameKey));

    private Type GetTypeFromTypeNameKey(TypeNameKey typeNameKey)
    {
      string assemblyName = typeNameKey.AssemblyName;
      string typeName = typeNameKey.TypeName;
      if (assemblyName == null)
        return Type.GetType(typeName);
      Assembly assembly1 = Assembly.LoadWithPartialName(assemblyName);
      if (assembly1 == (Assembly) null)
      {
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == assemblyName || assembly2.GetName().Name == assemblyName)
          {
            assembly1 = assembly2;
            break;
          }
        }
      }
      Type typeFromTypeNameKey = !(assembly1 == (Assembly) null) ? assembly1.GetType(typeName) : throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) assemblyName));
      if (typeFromTypeNameKey == (Type) null)
      {
        if (typeName.IndexOf('`') >= 0)
        {
          try
          {
            typeFromTypeNameKey = this.GetGenericTypeFromTypeName(typeName, assembly1);
          }
          catch (Exception ex)
          {
            throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) assembly1.FullName), ex);
          }
        }
        if (typeFromTypeNameKey == (Type) null)
          throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) assembly1.FullName));
      }
      return typeFromTypeNameKey;
    }

    private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
    {
      Type typeFromTypeName = (Type) null;
      int length = typeName.IndexOf('[');
      if (length >= 0)
      {
        string name = typeName.Substring(0, length);
        Type type = assembly.GetType(name);
        if (type != (Type) null)
        {
          List<Type> typeList = new List<Type>();
          int num1 = 0;
          int startIndex = 0;
          int num2 = typeName.Length - 1;
          for (int index = length + 1; index < num2; ++index)
          {
            switch (typeName[index])
            {
              case '[':
                if (num1 == 0)
                  startIndex = index + 1;
                ++num1;
                break;
              case ']':
                --num1;
                if (num1 == 0)
                {
                  TypeNameKey typeNameKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeName.Substring(startIndex, index - startIndex));
                  typeList.Add(this.GetTypeByName(typeNameKey));
                  break;
                }
                break;
            }
          }
          typeFromTypeName = type.MakeGenericType(typeList.ToArray());
        }
      }
      return typeFromTypeName;
    }

    private Type GetTypeByName(TypeNameKey typeNameKey) => this._typeCache.Get(typeNameKey);

    public override Type BindToType(string assemblyName, string typeName) => this.GetTypeByName(new TypeNameKey(assemblyName, typeName));

    public override void BindToName(
      Type serializedType,
      out string assemblyName,
      out string typeName)
    {
      assemblyName = serializedType.Assembly.FullName;
      typeName = serializedType.FullName;
    }
  }
}
