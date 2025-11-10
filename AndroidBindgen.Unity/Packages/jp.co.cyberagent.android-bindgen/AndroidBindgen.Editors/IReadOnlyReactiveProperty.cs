// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AndroidBindgen.Editors
{
    internal interface IReadOnlyReactiveProperty<out T>
    {
        T Value { get; }
        IDisposable Subscribe(Action<T, T> onNext);
    }
}
