﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlDeclaration
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlDeclaration : IXmlNode
  {
    string Version { get; }

    string Encoding { get; set; }

    string Standalone { get; set; }
  }
}