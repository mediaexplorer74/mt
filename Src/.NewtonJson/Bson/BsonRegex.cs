// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonRegex
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonRegex : BsonToken
  {
    public BsonString Pattern { get; set; }

    public BsonString Options { get; set; }

    public BsonRegex(string pattern, string options)
    {
      this.Pattern = new BsonString((object) pattern, false);
      this.Options = new BsonString((object) options, false);
    }

    public override BsonType Type => BsonType.Regex;
  }
}
