// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
  {
    private readonly XmlDeclaration _declaration;

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base((XmlNode) declaration)
    {
      this._declaration = declaration;
    }

    public string Version => this._declaration.Version;

    public string Encoding
    {
      get => this._declaration.Encoding;
      set => this._declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this._declaration.Standalone;
      set => this._declaration.Standalone = value;
    }
  }
}
