// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using Cecla;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    [Serializable]
    internal record JavaField
    {
        public JavaField(FieldInfo source)
        {
            Source = source;
            name = source.Name;
            descriptor = source.Descriptor;
            isDeprecated = source.Attributes.Span.Any(a => a is AttributeDeprecated);
        }

        public FieldInfo Source { get; }

        public string Name => name;
        public string Descriptor => descriptor;

        public string GenerationScopeKey => $"F:{name}";
        public bool IsDeprecated => isDeprecated;

        #region Serialized Fields

        [SerializeField] private string name;
        [SerializeField] private string descriptor;
        [SerializeField] private bool isDeprecated;

        #endregion
    }
}
