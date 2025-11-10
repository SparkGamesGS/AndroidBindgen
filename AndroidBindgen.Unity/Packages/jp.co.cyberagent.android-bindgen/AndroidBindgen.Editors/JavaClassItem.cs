// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace AndroidBindgen.Editors
{
    internal class JavaClassItem : ContainerItem
    {
        private readonly JavaClass _class;
        private readonly GenerationStatusScope _scope;

        public JavaClassItem(JavaClass @class, GenerationStatusScope scope)
        {
            _class = @class;
            _scope = scope;
            Children = CreateChildren().ToArray();
        }

        public override string Name => _class.Name;

        public override string Kind => _class.IsInterface ? "Interface" : "Class";

        public override string Signature { get; }
        public override IEnumerable<ITreeViewItem> Children { get; }

        public override bool IsDeprecated => _class.IsDeprecated;

        private IEnumerable<ITreeViewItem> CreateChildren()
        {
            var scope = _scope.Enter(_class.GenerationScopeKey);

            if (_class.IsInterface)
                yield return RegisterChild(new JavaProxyItem(scope, IsDeprecated));

            foreach (var field in _class.Fields)
                yield return RegisterChild(new JavaFieldItem(field, scope));
            foreach (var method in _class.Methods)
                yield return RegisterChild(new JavaMethodItem(method, scope));
        }
    }
}
