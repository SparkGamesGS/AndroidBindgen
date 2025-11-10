// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using Cecla;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    [Serializable]
    internal record JavaMethod
    {
        public JavaMethod(MethodInfo source)
        {
            Source = source;
            name = source.Name;
            descriptor = source.Descriptor;
            isDeprecated = source.Attributes.Span.Any(a => a is AttributeDeprecated);
        }

        public MethodInfo Source { get; }

        public string Name => name;
        public string Descriptor => descriptor;

        public string GenerationScopeKey => $"M:{Name}_{Descriptor}";
        public bool IsDeprecated => isDeprecated;

        #region Serialized Fields

        [SerializeField] private string name;
        [SerializeField] private string descriptor;
        [SerializeField] private bool isDeprecated;

        #endregion
    }
}
