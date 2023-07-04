// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonMergeSettings
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Linq
{
  public class JsonMergeSettings
  {
    private MergeArrayHandling _mergeArrayHandling;
    private MergeNullValueHandling _mergeNullValueHandling;

    public MergeArrayHandling MergeArrayHandling
    {
      get => this._mergeArrayHandling;
      set => this._mergeArrayHandling = value >= MergeArrayHandling.Concat && value <= MergeArrayHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public MergeNullValueHandling MergeNullValueHandling
    {
      get => this._mergeNullValueHandling;
      set => this._mergeNullValueHandling = value >= MergeNullValueHandling.Ignore && value <= MergeNullValueHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
