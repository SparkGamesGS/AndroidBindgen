// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal abstract class ContainerItem : ITreeViewItem
    {
        private readonly List<ITreeViewItem> _manipulatableChildren = new();

        private readonly ReactiveProperty<GenerationStatusKind> _status = new();
        private int _deprecatedItems;
        private int _maxStatusValue;
        private int _statusValue;

        private void UpdateStatus()
        {
            _status.Value = _statusValue == 0 ? GenerationStatusKind.Disabled :
                _statusValue == _maxStatusValue ? GenerationStatusKind.Enabled :
                GenerationStatusKind.Partial;
        }

        protected ITreeViewItem RegisterChild(ITreeViewItem item)
        {
            if (!item.IsManipulatable) return item;
            if (item.IsDeprecated) _deprecatedItems++;
            item.Status.Subscribe((oldValue, newValue) =>
            {
                var delta = newValue - oldValue;
                _statusValue += delta;
                UpdateStatus();
            });
            _statusValue += (int)item.Status.Value;
            _maxStatusValue += 2;
            UpdateStatus();
            _manipulatableChildren.Add(item);
            return item;
        }

        #region ITreeViewItem Members

        public abstract string Name { get; }
        public abstract string Kind { get; }
        public virtual string Type => _deprecatedItems > 0 ? $"{_deprecatedItems} deprecated items" : "";
        public abstract string Signature { get; }

        public virtual bool IsManipulatable => Children.Any(c => c.IsManipulatable);
        public abstract IEnumerable<ITreeViewItem> Children { get; }
        public IReadOnlyReactiveProperty<GenerationStatusKind> Status => _status;

        public void SetStatus(bool enabled, Func<ITreeViewItem, bool> filter)
        {
            foreach (var item in _manipulatableChildren)
                item.SetStatus(enabled, filter);
        }

        public virtual bool IsDeprecated => false;

        #endregion
    }
}
