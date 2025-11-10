// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AndroidBindgen.Editors
{
    internal interface ITreeViewItem
    {
        string Name { get; }
        string Kind { get; }
        string Type { get; }
        string Signature { get; }
        bool IsManipulatable { get; }
        IEnumerable<ITreeViewItem> Children { get; }
        IReadOnlyReactiveProperty<GenerationStatusKind> Status { get; }
        bool IsDeprecated { get; }
        void SetStatus(bool enabled, Func<ITreeViewItem, bool> filter);
    }
}
