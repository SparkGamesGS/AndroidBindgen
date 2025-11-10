// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen.Editors
{
    internal interface IGenerationStatusProvider
    {
        IReactiveProperty<bool> GetIsGenerated(string key);
    }

    internal readonly struct GenerationStatusScope
    {
        private readonly string _scope;
        private readonly IGenerationStatusProvider _generationStateProvider;

        public GenerationStatusScope(IGenerationStatusProvider generationStateProvider)
        {
            _scope = "";
            _generationStateProvider = generationStateProvider;
        }

        private GenerationStatusScope(string scope, IGenerationStatusProvider generationStateProvider)
        {
            _scope = scope;
            _generationStateProvider = generationStateProvider;
        }

        public GenerationStatusScope Enter(string scope)
        {
            return new GenerationStatusScope(_scope + "/" + scope, _generationStateProvider);
        }

        public IReactiveProperty<bool> GetIsGenerated()
        {
            return _generationStateProvider.GetIsGenerated(_scope);
        }
    }
}
