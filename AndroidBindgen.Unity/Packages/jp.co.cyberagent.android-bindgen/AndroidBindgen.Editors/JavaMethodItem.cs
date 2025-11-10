// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaMethodItem : ITreeViewItem
    {
        private readonly JavaMethod _method;

        private readonly IReactiveProperty<bool> _status;

        public JavaMethodItem(JavaMethod method, GenerationStatusScope scope)
        {
            _method = method;

            var status = _status = scope.Enter(_method.GenerationScopeKey).GetIsGenerated();
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

        public string Name => _method.Name;
        public string Kind => "Method";
        public string Type => _method.Descriptor;
        public string Signature { get; }
        public bool IsManipulatable => true;
        public IEnumerable<ITreeViewItem> Children => Enumerable.Empty<ITreeViewItem>();
        public IReadOnlyReactiveProperty<GenerationStatusKind> Status { get; }

        public void SetStatus(bool enabled, Func<ITreeViewItem, bool> filter)
        {
            if (!filter(this) && enabled) return;
            _status.Value = enabled;
        }

        public bool IsDeprecated => _method.IsDeprecated;

        #endregion
    }
}
