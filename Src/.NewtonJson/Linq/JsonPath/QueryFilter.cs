﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryFilter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class QueryFilter : PathFilter
  {
    public QueryExpression Expression { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (IEnumerable<JToken> jtokens in current)
      {
        foreach (JToken t in jtokens)
        {
          if (this.Expression.IsMatch(root, t))
            yield return t;
        }
      }
    }
  }
}
