// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal record struct ParameterInfo(IParameterType Type, string Name, bool IsNullable)
{
    public string CSharpExpression => $"{Type.GetCSharpExpression(IsNullable)}";
}
