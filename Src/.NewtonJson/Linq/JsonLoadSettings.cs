// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonLoadSettings
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Linq
{
  public class JsonLoadSettings
  {
    private CommentHandling _commentHandling;
    private LineInfoHandling _lineInfoHandling;

    public JsonLoadSettings()
    {
      this._lineInfoHandling = LineInfoHandling.Load;
      this._commentHandling = CommentHandling.Ignore;
    }

    public CommentHandling CommentHandling
    {
      get => this._commentHandling;
      set => this._commentHandling = value >= CommentHandling.Ignore && value <= CommentHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public LineInfoHandling LineInfoHandling
    {
      get => this._lineInfoHandling;
      set => this._lineInfoHandling = value >= LineInfoHandling.Ignore && value <= LineInfoHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
