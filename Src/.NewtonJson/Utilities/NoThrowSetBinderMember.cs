// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowSetBinderMember
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Dynamic;

namespace Newtonsoft.Json.Utilities
{
  internal class NoThrowSetBinderMember : SetMemberBinder
  {
    private readonly SetMemberBinder _innerBinder;

    public NoThrowSetBinderMember(SetMemberBinder innerBinder)
      : base(innerBinder.Name, innerBinder.IgnoreCase)
    {
      this._innerBinder = innerBinder;
    }

    public override DynamicMetaObject FallbackSetMember(
      DynamicMetaObject target,
      DynamicMetaObject value,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[1]
      {
        value
      });
      return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
    }
  }
}
