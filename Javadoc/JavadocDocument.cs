// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Javadoc;

public class JavadocDocument
{
    public JavadocDocument(XmlReader reader)
    {
        Api = new XmlSerializer(typeof(Api)).Deserialize(reader) as Api;
    }

    public Api? Api { get; }
}
