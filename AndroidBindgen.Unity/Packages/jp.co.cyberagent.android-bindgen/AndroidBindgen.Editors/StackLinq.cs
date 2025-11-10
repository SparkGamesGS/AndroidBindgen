// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

#nullable enable

using System;

namespace AndroidBindgen.Editors
{
    internal static class StackLinq
    {
        public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item))
                    return true;

            return false;
        }

        public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate, out T? element)
        {
            foreach (var item in source)
                if (predicate(item))
                {
                    element = item;
                    return true;
                }

            element = default;
            return false;
        }
    }
}
