// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanFilter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ScanFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken c in current)
      {
        if (this.Name == null)
          yield return c;
        JToken value = c;
        JToken jtoken = c;
        while (true)
        {
          if (jtoken != null && jtoken.HasValues)
          {
            value = jtoken.First;
          }
          else
          {
            while (value != null && value != c && value == value.Parent.Last)
              value = (JToken) value.Parent;
            if (value != null && value != c)
              value = value.Next;
            else
              break;
          }
          if (value is JProperty jproperty)
          {
            if (jproperty.Name == this.Name)
              yield return jproperty.Value;
          }
          else if (this.Name == null)
            yield return value;
          jtoken = (JToken) (value as JContainer);
        }
        value = (JToken) null;
      }
    }
  }
}
