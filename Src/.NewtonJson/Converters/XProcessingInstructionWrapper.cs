// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XProcessingInstructionWrapper : XObjectWrapper
  {
    private XProcessingInstruction ProcessingInstruction => (XProcessingInstruction) this.WrappedNode;

    public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
      : base((XObject) processingInstruction)
    {
    }

    public override string LocalName => this.ProcessingInstruction.Target;

    public override string Value
    {
      get => this.ProcessingInstruction.Data;
      set => this.ProcessingInstruction.Data = value;
    }
  }
}
