// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlDocument
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlDocument : IXmlNode
  {
    IXmlNode CreateComment(string text);

    IXmlNode CreateTextNode(string text);

    IXmlNode CreateCDataSection(string data);

    IXmlNode CreateWhitespace(string text);

    IXmlNode CreateSignificantWhitespace(string text);

    IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

    IXmlNode CreateXmlDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset);

    IXmlNode CreateProcessingInstruction(string target, string data);

    IXmlElement CreateElement(string elementName);

    IXmlElement CreateElement(string qualifiedName, string namespaceUri);

    IXmlNode CreateAttribute(string name, string value);

    IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);

    IXmlElement DocumentElement { get; }
  }
}
