// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using Javadoc;

namespace AndroidBindgen;

public class JavadocManager
{
    private readonly Dictionary<string, Class> _classes;

    public JavadocManager(string[] javadocXmlPaths)
    {
        List<Class> classes = new();
        foreach (var jd in javadocXmlPaths)
        {
            using var file = File.OpenRead(jd);
            using var reader = new XmlTextReader(file);
            var doc = new JavadocDocument(reader);
            foreach (var package in doc.Api?.Packages ?? Array.Empty<Package>())
            {
                classes.AddRange(package.Classes ?? Array.Empty<Class>());
                classes.AddRange(package.Interfaces ?? Array.Empty<Interface>());
            }
        }

        _classes = classes.ToDictionary(c =>
        {
            // signature to name
            var sig = c.JniSignature ?? throw new InvalidOperationException();
            if (!sig.StartsWith("L") || !sig.EndsWith(";")) throw new InvalidOperationException();

            return sig[1..^1];
        });
    }

    public Class GetClass(string className)
    {
        var @class = _classes[className];
        return @class;
    }

    public bool TryGetClass(string className, [NotNullWhen(true)] out Class? @class)
    {
        return _classes.TryGetValue(className, out @class);
    }
}
