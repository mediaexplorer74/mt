// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonEmpty
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonEmpty : BsonToken
  {
    public static readonly BsonToken Null = (BsonToken) new BsonEmpty(BsonType.Null);
    public static readonly BsonToken Undefined = (BsonToken) new BsonEmpty(BsonType.Undefined);

    private BsonEmpty(BsonType type) => this.Type = type;

    public override BsonType Type { get; }
  }
}
