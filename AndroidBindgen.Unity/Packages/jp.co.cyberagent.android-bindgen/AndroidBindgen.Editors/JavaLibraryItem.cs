// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaLibraryItem : ContainerItem
    {
        private readonly JavaLibrary _library;
        private readonly GenerationStatusScope _scope;

        public JavaLibraryItem(JavaLibrary library, GenerationStatusScope scope)
        {
            _library = library;
            _scope = scope;
            Children = CreateChildren().ToArray();
        }

        public override string Name => _library.Name;
        public override string Kind => "Library";
        public override string Signature => "";
        public override IEnumerable<ITreeViewItem> Children { get; }
        public override bool IsManipulatable => false;

        private IEnumerable<ITreeViewItem> CreateChildren()
        {
            var scope = _scope.Enter(_library.GenerationScopeKey);
            foreach (var package in _library.Packages)
                yield return RegisterChild(new JavaPackageItem(package, scope));
        }
    }
}
