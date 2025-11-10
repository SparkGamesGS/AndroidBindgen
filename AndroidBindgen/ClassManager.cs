// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cecla;

namespace AndroidBindgen;

public class ClassManager
{
    private readonly IEnumerable<JavaArchive> _archives;
    private readonly Dictionary<string, ClassFile> _loadedClasses = new();

    public ClassManager(IEnumerable<JavaArchive> archives)
    {
        _archives = archives.ToArray();
    }

    public bool TryGetClass(string name, [NotNullWhen(true)] out ClassFile? loaded)
    {
        if (!_loadedClasses.TryGetValue(name, out loaded))
        {
            foreach (var archive in _archives)
            {
                var found = archive.GetClass(name);
                if (found != null)
                {
                    _loadedClasses[name] = loaded = found;
                    goto FOUND;
                }
            }

            loaded = null;
            return false;
        }

        FOUND:
        return true;
    }
}
