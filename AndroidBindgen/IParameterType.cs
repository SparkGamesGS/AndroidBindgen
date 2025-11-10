// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal interface IParameterType
{
    bool IsVoid { get; }
    string JavaArrayClassCSharpExpression { get; }
    string GetCSharpExpression(bool isNullable);
    string CallMethodNameWithReturn(bool isStatic);
    string GetFieldMethodName(bool isStatic);
    string SetFieldMethodName(bool isStatic);
    string JValueInitializer(MarshalingContext context, string source);
    string ToJavaArray(MarshalingContext? context, string source);
    string FromJavaArray(string source);
    string ToCSharpValue(string source);
    string FromCSharpValue(string source);
    string GetProxyParameterMatcher(string classNameSource, string classPointerSource, bool isBoxed);
    string GetProxyUnboxer(MarshalingContext context, string source);
    string GetProxyBoxer(string source);
}
