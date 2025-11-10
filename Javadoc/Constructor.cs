// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class Constructor
{
    [XmlAttribute("jni-signature")] public string? JniSignature { get; set; }
    [XmlElement("parameter")] public Parameter[]? Parameters { get; set; }
    [XmlElement("javadoc")] public XmlCDataSection? Javadoc { get; set; }
}
