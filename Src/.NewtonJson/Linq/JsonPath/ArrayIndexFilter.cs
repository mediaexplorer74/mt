// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayIndexFilter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ArrayIndexFilter : PathFilter
  {
    public int? Index { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        if (this.Index.HasValue)
        {
          JToken tokenIndex = PathFilter.GetTokenIndex(t, errorWhenNoMatch, this.Index.GetValueOrDefault());
          if (tokenIndex != null)
            yield return tokenIndex;
        }
        else if (t is JArray || t is JConstructor)
        {
          foreach (JToken jtoken in (IEnumerable<JToken>) t)
            yield return jtoken;
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Index * not valid on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) t.GetType().Name));
      }
    }
  }
}
