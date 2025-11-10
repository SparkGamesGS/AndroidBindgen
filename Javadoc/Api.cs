// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml.Serialization;

namespace Javadoc;

[XmlRoot("api")]
public class Api
{
    [XmlAttribute("api-source")] public string? ApiSource { get; set; }
    [XmlElement("package")] public Package[]? Packages { get; set; }
}
