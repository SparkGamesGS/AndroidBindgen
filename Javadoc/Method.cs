// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class Method
{
    [XmlAttribute("jni-signature")] public string? JniSignature { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("jni-return")] public string? JniReturn { get; set; }
    [XmlAttribute("return")] public string? Return { get; set; }
    [XmlElement("parameter")] public Parameter[]? Parameters { get; set; }
    [XmlElement("javadoc")] public XmlCDataSection? Javadoc { get; set; }
}
