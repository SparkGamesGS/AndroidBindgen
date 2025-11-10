// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaPackageItem : ContainerItem
    {
        private readonly JavaPackage _package;
        private readonly GenerationStatusScope _scope;

        public JavaPackageItem(JavaPackage package, GenerationStatusScope scope)
        {
            _package = package;
            _scope = scope;
            Children = CreateChildren().ToArray();
        }

        public override string Name => _package.Name;
        public override string Kind => "Package";
        public override string Signature => "";
        public override IEnumerable<ITreeViewItem> Children { get; }

        private IEnumerable<ITreeViewItem> CreateChildren()
        {
            var scope = _scope.Enter(_package.GenerationScopeKey);
            foreach (var @class in _package.Classes)
                yield return RegisterChild(new JavaClassItem(@class, scope));
        }
    }
}
