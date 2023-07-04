// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
  {
    private readonly XmlDocumentType _documentType;

    public XmlDocumentTypeWrapper(XmlDocumentType documentType)
      : base((XmlNode) documentType)
    {
      this._documentType = documentType;
    }

    public string Name => this._documentType.Name;

    public string System => this._documentType.SystemId;

    public string Public => this._documentType.PublicId;

    public string InternalSubset => this._documentType.InternalSubset;

    public override string LocalName => "DOCTYPE";
  }
}
