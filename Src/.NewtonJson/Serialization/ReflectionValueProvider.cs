﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionValueProvider
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class ReflectionValueProvider : IValueProvider
  {
    private readonly MemberInfo _memberInfo;

    public ReflectionValueProvider(MemberInfo memberInfo)
    {
      ValidationUtils.ArgumentNotNull((object) memberInfo, nameof (memberInfo));
      this._memberInfo = memberInfo;
    }

    public void SetValue(object target, object value)
    {
      try
      {
        ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }

    public object GetValue(object target)
    {
      try
      {
        return ReflectionUtils.GetMemberValue(this._memberInfo, target);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }
  }
}