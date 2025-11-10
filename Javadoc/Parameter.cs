// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class Parameter
{
    [XmlAttribute("jni-type")] public string? JniType { get; set; }
    [XmlAttribute("type")] public string? Type { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("javadoc")] public XmlCDataSection? Javadoc { get; set; }
}
