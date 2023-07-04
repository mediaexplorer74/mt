// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonToken
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Bson
{
  internal abstract class BsonToken
  {
    public abstract BsonType Type { get; }

    public BsonToken Parent { get; set; }

    public int CalculatedSize { get; set; }
  }
}
