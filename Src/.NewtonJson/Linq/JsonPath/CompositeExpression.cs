// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.CompositeExpression
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class CompositeExpression : QueryExpression
  {
    public List<QueryExpression> Expressions { get; set; }

    public CompositeExpression() => this.Expressions = new List<QueryExpression>();

    public override bool IsMatch(JToken root, JToken t)
    {
      switch (this.Operator)
      {
        case QueryOperator.And:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (!expression.IsMatch(root, t))
              return false;
          }
          return true;
        case QueryOperator.Or:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (expression.IsMatch(root, t))
              return true;
          }
          return false;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
