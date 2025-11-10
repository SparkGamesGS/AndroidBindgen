// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cecla;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    [Serializable]
    internal record JavaLibrary
    {
        public JavaLibrary(JavaArchive archive, string name)
        {
            var classesByPackages = archive.GetClasses()
                .Where(c => c.@class.AccessFlags.HasFlag(AccessFlags.Public)).GroupBy(entry =>
                {
                    var del = entry.name.IndexOf('$');
                    var rootClass = del == -1 ? entry.name : entry.name.Substring(0, del);
                    var del2 = rootClass.LastIndexOf('/');
                    return del2 == -1 ? rootClass : rootClass.Substring(0, del2);
                });

            List<JavaPackage> packages = new();
            foreach (var package in classesByPackages)
                packages.Add(new JavaPackage(package.Key.Replace('/', '.'), package
                    .OrderBy(entry => entry.name).Select(entry =>
                    {
                        var name = entry.name;
                        var del = name.LastIndexOf('/'); // leave nested class delimiter '$'
                        name = del == -1 ? name : name.Substring(del + 1);
                        return (entry.@class, name);
                    })));

            this.packages = packages.ToArray();
            this.name = name;
        }

        public string GenerationScopeKey => $"L:{name}";

        public IEnumerable<JavaPackage> Packages => packages;
        public string Name => name;

        #region Serialized Fields

        [SerializeField] private JavaPackage[] packages;
        [SerializeField] private string name;

        #endregion
    }
}
