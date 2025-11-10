// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaProxyItem : ITreeViewItem
    {
        private readonly IReactiveProperty<bool> _status;

        public JavaProxyItem(GenerationStatusScope scope, bool isDeprecated)
        {
            IsDeprecated = isDeprecated;
            var status = _status = scope.Enter("I:Proxy").GetIsGenerated();
            var s = new ReactiveProperty<GenerationStatusKind>(status.Value
                ? GenerationStatusKind.Enabled
                : GenerationStatusKind.Disabled);
            Status = s;
            status.Subscribe((_, newValue) =>
            {
                s.Value = newValue ? GenerationStatusKind.Enabled : GenerationStatusKind.Disabled;
            });
        }

        #region ITreeViewItem Members

        public string Name => "Generate Proxy for Interface";
        public string Kind => "";
        public string Type => "";
        public string Signature => "";
        public bool IsManipulatable => true;

        public IEnumerable<ITreeViewItem> Children => Enumerable.Empty<ITreeViewItem>();
        public IReadOnlyReactiveProperty<GenerationStatusKind> Status { get; }

        public void SetStatus(bool enabled, Func<ITreeViewItem, bool> filter)
        {
            if (!filter(this) && enabled) return;
            _status.Value = enabled;
        }

        public bool IsDeprecated { get; }

        #endregion
    }
}
