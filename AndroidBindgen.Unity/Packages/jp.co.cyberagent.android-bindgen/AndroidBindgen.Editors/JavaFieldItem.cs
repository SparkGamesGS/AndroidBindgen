// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaFieldItem : ITreeViewItem
    {
        private readonly JavaField _field;

        private readonly IReactiveProperty<bool> _status;

        public JavaFieldItem(JavaField field, GenerationStatusScope scope)
        {
            _field = field;

            var status = _status = scope.Enter(_field.GenerationScopeKey).GetIsGenerated();
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

        public string Name => _field.Name;
        public string Kind => "Field";
        public string Type => _field.Descriptor;
        public string Signature => _field.Descriptor;
        public bool IsManipulatable => true;

        public IEnumerable<ITreeViewItem> Children => Enumerable.Empty<ITreeViewItem>();
        public IReadOnlyReactiveProperty<GenerationStatusKind> Status { get; }

        public void SetStatus(bool enabled, Func<ITreeViewItem, bool> filter)
        {
            if (!filter(this) && enabled) return;
            _status.Value = enabled;
        }

        public bool IsDeprecated => _field.IsDeprecated;

        #endregion
    }
}
