// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBoolean
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonBoolean : BsonValue
  {
    public static readonly BsonBoolean False = new BsonBoolean(false);
    public static readonly BsonBoolean True = new BsonBoolean(true);

    private BsonBoolean(bool value)
      : base((object) value, BsonType.Boolean)
    {
    }
  }
}
