// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JRaw
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  public class JRaw : JValue
  {
    public JRaw(JRaw other)
      : base((JValue) other)
    {
    }

    public JRaw(object rawJson)
      : base(rawJson, JTokenType.Raw)
    {
    }

    public static JRaw Create(JsonReader reader)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
        {
          jsonTextWriter.WriteToken(reader);
          return new JRaw((object) stringWriter.ToString());
        }
      }
    }

    internal override JToken CloneToken() => (JToken) new JRaw(this);
  }
}
