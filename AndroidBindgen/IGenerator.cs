// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal interface IGenerator
{
    void Request(string className, IApiFilter filter);
}
