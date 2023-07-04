// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XContainerWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XContainerWrapper : XObjectWrapper
  {
    private List<IXmlNode> _childNodes;

    private XContainer Container => (XContainer) this.WrappedNode;

    public XContainerWrapper(XContainer container)
      : base((XObject) container)
    {
    }

    public override List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          if (!this.HasChildNodes)
          {
            this._childNodes = XmlNodeConverter.EmptyChildNodes;
          }
          else
          {
            this._childNodes = new List<IXmlNode>();
            foreach (XObject node in this.Container.Nodes())
              this._childNodes.Add(XContainerWrapper.WrapNode(node));
          }
        }
        return this._childNodes;
      }
    }

    protected virtual bool HasChildNodes => this.Container.LastNode != null;

    public override IXmlNode ParentNode => this.Container.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Container.Parent);

    internal static IXmlNode WrapNode(XObject node)
    {
      switch (node)
      {
        case XDocument document:
          return (IXmlNode) new XDocumentWrapper(document);
        case XElement element:
          return (IXmlNode) new XElementWrapper(element);
        case XContainer container:
          return (IXmlNode) new XContainerWrapper(container);
        case XProcessingInstruction processingInstruction:
          return (IXmlNode) new XProcessingInstructionWrapper(processingInstruction);
        case XText text1:
          return (IXmlNode) new XTextWrapper(text1);
        case XComment text2:
          return (IXmlNode) new XCommentWrapper(text2);
        case XAttribute attribute:
          return (IXmlNode) new XAttributeWrapper(attribute);
        case XDocumentType documentType:
          return (IXmlNode) new XDocumentTypeWrapper(documentType);
        default:
          return (IXmlNode) new XObjectWrapper(node);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      this.Container.Add(newChild.WrappedNode);
      this._childNodes = (List<IXmlNode>) null;
      return newChild;
    }
  }
}
