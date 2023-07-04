// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObject
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
  internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
  {
    private readonly List<BsonProperty> _children = new List<BsonProperty>();

    public void Add(string name, BsonToken token)
    {
      this._children.Add(new BsonProperty()
      {
        Name = new BsonString((object) name, false),
        Value = token
      });
      token.Parent = (BsonToken) this;
    }

    public override BsonType Type => BsonType.Object;

    public IEnumerator<BsonProperty> GetEnumerator() => (IEnumerator<BsonProperty>) this._children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
