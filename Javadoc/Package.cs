// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml.Serialization;

namespace Javadoc;

public class Package
{
    [XmlAttribute("jni-name")] public string? JniName { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("class")] public Class[]? Classes { get; set; }
    [XmlElement("interface")] public Interface[]? Interfaces { get; set; }
}
