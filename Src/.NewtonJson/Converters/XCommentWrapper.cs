// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XCommentWrapper : XObjectWrapper
  {
    private XComment Text => (XComment) this.WrappedNode;

    public XCommentWrapper(XComment text)
      : base((XObject) text)
    {
    }

    public override string Value
    {
      get => this.Text.Value;
      set => this.Text.Value = value;
    }

    public override IXmlNode ParentNode => this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
  }
}
