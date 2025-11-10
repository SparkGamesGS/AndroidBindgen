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
    internal record JavaPackage
    {
        public JavaPackage(string name, IEnumerable<(ClassFile @class, string name)> classes)
        {
            this.classes = classes.Select(c => new JavaClass(c.name, c.@class)).ToArray();
            this.name = name;
        }

        public IEnumerable<JavaClass> Classes => classes;
        public string Name => name;
        public string GenerationScopeKey => $"P:{name}";

        #region Serialized Fields

        [SerializeField] private JavaClass[] classes;
        [SerializeField] private string name;

        #endregion
    }
}
