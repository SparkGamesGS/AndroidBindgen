// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using Cecla;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    [Serializable]
    internal record JavaClass
    {
        public JavaClass(string name, ClassFile source)
        {
            Source = source;
            List<JavaField> fields = new();
            for (var i = 0; i < source.Fields.Span.Length; i++)
            {
                var field = source.Fields.Span[i];
                if (!field.AccessFlags.HasFlag(AccessFlags.Public)) continue;
                fields.Add(new JavaField(field));
            }

            List<JavaMethod> methods = new();
            for (var i = 0; i < source.Methods.Span.Length; i++)
            {
                var method = source.Methods.Span[i];
                if (!method.AccessFlags.HasFlag(AccessFlags.Public)) continue;
                methods.Add(new JavaMethod(method));
            }

            this.fields = fields.ToArray();
            this.methods = methods.ToArray();
            this.name = name.Replace('/', '.').Replace('$', '.');
            isInterface = source.AccessFlags.HasFlag(AccessFlags.Interface);
            isDeprecated = source.Attributes.Span.Any(attr => attr is AttributeDeprecated);
        }

        public ClassFile Source { get; }

        public IEnumerable<JavaField> Fields => fields;

        public IEnumerable<JavaMethod> Methods => methods;

        public string Name => name;
        public bool IsInterface => isInterface;
        public string Descriptor => "";

        public string GenerationScopeKey => $"C:{name}";
        public bool IsDeprecated => isDeprecated;

        #region Serialized Fields

        [SerializeField] private JavaField[] fields;
        [SerializeField] private JavaMethod[] methods;
        [SerializeField] private string name;
        [SerializeField] private bool isInterface;
        [SerializeField] private bool isDeprecated;

        #endregion
    }
}
