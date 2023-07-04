// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaResolver
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See http://www.newtonsoft.com/jsonschema for more details.")]
  public class JsonSchemaResolver
  {
    public IList<JsonSchema> LoadedSchemas { get; protected set; }

    public JsonSchemaResolver() => this.LoadedSchemas = (IList<JsonSchema>) new List<JsonSchema>();

    public virtual JsonSchema GetSchema(string reference) => this.LoadedSchemas.SingleOrDefault<JsonSchema>((Func<JsonSchema, bool>) (s => string.Equals(s.Id, reference, StringComparison.Ordinal))) ?? this.LoadedSchemas.SingleOrDefault<JsonSchema>((Func<JsonSchema, bool>) (s => string.Equals(s.Location, reference, StringComparison.Ordinal)));
  }
}
