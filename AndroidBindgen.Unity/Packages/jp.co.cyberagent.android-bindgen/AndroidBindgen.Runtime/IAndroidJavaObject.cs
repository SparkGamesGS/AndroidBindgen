// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

#nullable enable

using System;

namespace AndroidBindgen
{
    public interface IAndroidJavaObject : IDisposable
    {
        IntPtr GetRawObject();
    }
}
