// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Threading;

namespace AndroidBindgen.Editors
{
    internal static class Disposable
    {
        public static readonly IDisposable Empty = Create<object>(null, _ => { });

        public static IDisposable Create<T>(T state, Action<T> dispose)
        {
            return new Core<T>(state, dispose);
        }

        #region Nested type: Core

        internal class Core<T> : IDisposable
        {
            private readonly T _state;
            private Action<T> _dispose;

            public Core(T state, Action<T> dispose)
            {
                _state = state;
                _dispose = dispose;
            }

            #region IDisposable Members

            public void Dispose()
            {
                var dispose = Interlocked.Exchange(ref _dispose, null);
                dispose?.Invoke(_state);
            }

            #endregion
        }

        #endregion
    }
}
