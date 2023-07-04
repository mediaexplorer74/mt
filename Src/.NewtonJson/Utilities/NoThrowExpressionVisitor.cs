// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowExpressionVisitor
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
  internal class NoThrowExpressionVisitor : ExpressionVisitor
  {
    internal static readonly object ErrorResult = new object();

    protected override Expression VisitConditional(ConditionalExpression node) => node.IfFalse.NodeType == ExpressionType.Throw ? (Expression) Expression.Condition(node.Test, node.IfTrue, (Expression) Expression.Constant(NoThrowExpressionVisitor.ErrorResult)) : base.VisitConditional(node);
  }
}
