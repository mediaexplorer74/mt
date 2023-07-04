// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private readonly XObject _xmlObject;

    public XObjectWrapper(XObject xmlObject) => this._xmlObject = xmlObject;

    public object WrappedNode => (object) this._xmlObject;

    public virtual XmlNodeType NodeType => this._xmlObject.NodeType;

    public virtual string LocalName => (string) null;

    public virtual List<IXmlNode> ChildNodes => XmlNodeConverter.EmptyChildNodes;

    public virtual List<IXmlNode> Attributes => XmlNodeConverter.EmptyChildNodes;

    public virtual IXmlNode ParentNode => (IXmlNode) null;

    public virtual string Value
    {
      get => (string) null;
      set => throw new InvalidOperationException();
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild) => throw new InvalidOperationException();

    public virtual string NamespaceUri => (string) null;
  }
}
