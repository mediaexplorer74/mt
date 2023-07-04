// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.RootFilter
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class RootFilter : PathFilter
  {
    public static readonly RootFilter Instance = new RootFilter();

    private RootFilter()
    {
    }

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      return (IEnumerable<JToken>) root;
    }
  }
}
