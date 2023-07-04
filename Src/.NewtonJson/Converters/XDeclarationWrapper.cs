// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
  {
    internal XDeclaration Declaration { get; }

    public XDeclarationWrapper(XDeclaration declaration)
      : base((XObject) null)
    {
      this.Declaration = declaration;
    }

    public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

    public string Version => this.Declaration.Version;

    public string Encoding
    {
      get => this.Declaration.Encoding;
      set => this.Declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this.Declaration.Standalone;
      set => this.Declaration.Standalone = value;
    }
  }
}
