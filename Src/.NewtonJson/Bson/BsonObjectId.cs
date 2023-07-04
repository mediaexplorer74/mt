// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Bson
{
  [Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
  public class BsonObjectId
  {
    public byte[] Value { get; }

    public BsonObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      this.Value = value.Length == 12 ? value : throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
    }
  }
}
