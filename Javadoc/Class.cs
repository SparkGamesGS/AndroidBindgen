// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class Class
{
    [XmlAttribute("jni-signature")] public string? JniSignature { get; set; }
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlElement("method")] public Method[]? Methods { get; set; }
    [XmlElement("constructor")] public Constructor[]? Constructors { get; set; }
    [XmlElement("field")] public Field[]? Fields { get; set; }
    [XmlElement("javadoc")] public XmlCDataSection? Javadoc { get; set; }
}
