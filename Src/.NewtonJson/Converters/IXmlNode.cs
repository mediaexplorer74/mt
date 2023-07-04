// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }

    string LocalName { get; }

    List<IXmlNode> ChildNodes { get; }

    List<IXmlNode> Attributes { get; }

    IXmlNode ParentNode { get; }

    string Value { get; set; }

    IXmlNode AppendChild(IXmlNode newChild);

    string NamespaceUri { get; }

    object WrappedNode { get; }
  }
}
