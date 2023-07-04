// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XAttributeWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XAttributeWrapper : XObjectWrapper
  {
    private XAttribute Attribute => (XAttribute) this.WrappedNode;

    public XAttributeWrapper(XAttribute attribute)
      : base((XObject) attribute)
    {
    }

    public override string Value
    {
      get => this.Attribute.Value;
      set => this.Attribute.Value = value;
    }

    public override string LocalName => this.Attribute.Name.LocalName;

    public override string NamespaceUri => this.Attribute.Name.NamespaceName;

    public override IXmlNode ParentNode => this.Attribute.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Attribute.Parent);
  }
}
