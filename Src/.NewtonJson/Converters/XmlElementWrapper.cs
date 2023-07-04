// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlElementWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
  {
    private readonly XmlElement _element;

    public XmlElementWrapper(XmlElement element)
      : base((XmlNode) element)
    {
      this._element = element;
    }

    public void SetAttributeNode(IXmlNode attribute) => this._element.SetAttributeNode((XmlAttribute) ((XmlNodeWrapper) attribute).WrappedNode);

    public string GetPrefixOfNamespace(string namespaceUri) => this._element.GetPrefixOfNamespace(namespaceUri);

    public bool IsEmpty => this._element.IsEmpty;
  }
}
