// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class Field
{
    [XmlAttribute("jni-signature")] public string? JniSignature { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("javadoc")] public XmlCDataSection? Javadoc { get; set; }
}
