// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AndroidBindgen;

internal readonly struct ParameterTypeImmediate : IParameterType
{
    public ParameterTypeImmediate(ParamTypeKind kind)
    {
        Kind = kind;
    }

    public ParamTypeKind Kind { get; }

    public string JavaArrayClassCSharpExpression =>
        $"global::UnityEngine.AndroidJNI.FindClass(\"[{Kind switch
        {
            ParamTypeKind.SByte => "B",
            ParamTypeKind.Char => "C",
            ParamTypeKind.Double => "D",
            ParamTypeKind.Float => "F",
            ParamTypeKind.Int => "I",
            ParamTypeKind.Long => "J",
            ParamTypeKind.Short => "S",
            ParamTypeKind.Boolean => "Z",
            ParamTypeKind.Void => throw new InvalidOperationException(),
            _ => throw new ArgumentOutOfRangeException()
        }}\")";

    public string CallMethodNameWithReturn(bool isStatic)
    {
        return $"Call{(isStatic ? "Static" : "")}{Kind}Method";
    }

    public string GetFieldMethodName(bool isStatic)
    {
        return $"Get{(isStatic ? "Static" : "")}{Kind}Field";
    }

    public string SetFieldMethodName(bool isStatic)
    {
        return $"Set{(isStatic ? "Static" : "")}{Kind}Field";
    }

    public bool IsVoid => Kind == ParamTypeKind.Void;

    public string GetCSharpExpression(bool isNullable)
    {
        return Kind switch
        {
            ParamTypeKind.SByte => "sbyte",
            ParamTypeKind.Char => "char",
            ParamTypeKind.Double => "double",
            ParamTypeKind.Float => "float",
            ParamTypeKind.Int => "int",
            ParamTypeKind.Long => "long",
            ParamTypeKind.Short => "short",
            ParamTypeKind.Boolean => "bool",
            ParamTypeKind.Void => "void",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string JValueInitializer(MarshalingContext context, string source)
    {
        return Kind switch
        {
            ParamTypeKind.SByte => $"b = {source}",
            ParamTypeKind.Char => $"c = {source}",
            ParamTypeKind.Double => $"d = {source}",
            ParamTypeKind.Float => $"f = {source}",
            ParamTypeKind.Int => $"i = {source}",
            ParamTypeKind.Long => $"j = {source}",
            ParamTypeKind.Short => $"s = {source}",
            ParamTypeKind.Boolean => $"z = {source}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string ToJavaArray(MarshalingContext? context, string source)
    {
        return Kind switch
        {
            ParamTypeKind.SByte => $"global::UnityEngine.AndroidJNI.ToSByteArray({source})",
            ParamTypeKind.Char => $"global::UnityEngine.AndroidJNI.ToCharArray({source})",
            ParamTypeKind.Double => $"global::UnityEngine.AndroidJNI.ToDoubleArray({source})",
            ParamTypeKind.Float => $"global::UnityEngine.AndroidJNI.ToFloatArray({source})",
            ParamTypeKind.Int => $"global::UnityEngine.AndroidJNI.ToIntArray({source})",
            ParamTypeKind.Long => $"global::UnityEngine.AndroidJNI.ToLongArray({source})",
            ParamTypeKind.Short => $"global::UnityEngine.AndroidJNI.ToShortArray({source})",
            ParamTypeKind.Boolean => $"global::AndroidBindgen.AndroidJNIEx.ToBooleanArray({source})",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string FromJavaArray(string source)
    {
        return Kind switch
        {
            ParamTypeKind.SByte => $"global::AndroidBindgen.AndroidJNIEx.FromSByteArray({source})",
            ParamTypeKind.Char => $"global::AndroidBindgen.AndroidJNIEx.FromCharArray({source})",
            ParamTypeKind.Double => $"global::AndroidBindgen.AndroidJNIEx.FromDoubleArray({source})",
            ParamTypeKind.Float => $"global::AndroidBindgen.AndroidJNIEx.FromFloatArray({source})",
            ParamTypeKind.Int => $"global::AndroidBindgen.AndroidJNIEx.FromIntArray({source})",
            ParamTypeKind.Long => $"global::AndroidBindgen.AndroidJNIEx.FromLongArray({source})",
            ParamTypeKind.Short => $"global::AndroidBindgen.AndroidJNIEx.FromShortArray({source})",
            ParamTypeKind.Boolean => $"global::AndroidBindgen.AndroidJNIEx.FromBooleanArray({source})",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string ToCSharpValue(string source)
    {
        return source;
    }

    public string FromCSharpValue(string source)
    {
        return source;
    }

    public string GetProxyParameterMatcher(string classNameSource, string classPointerSource, bool isBoxed)
    {
        return $"{classNameSource} == \"{(isBoxed ? Kind switch
        {
            ParamTypeKind.SByte => "java.lang.Byte",
            ParamTypeKind.Char => "java.lang.Character",
            ParamTypeKind.Double => "java.lang.Double",
            ParamTypeKind.Float => "java.lang.Float",
            ParamTypeKind.Int => "java.lang.Integer",
            ParamTypeKind.Long => "java.lang.Long",
            ParamTypeKind.Short => "java.lang.Short",
            ParamTypeKind.Boolean => "java.lang.Boolean",
            _ => throw new ArgumentOutOfRangeException()
        } : Kind switch
        {
            ParamTypeKind.SByte => "byte",
            ParamTypeKind.Char => "char",
            ParamTypeKind.Double => "double",
            ParamTypeKind.Float => "float",
            ParamTypeKind.Int => "int",
            ParamTypeKind.Long => "long",
            ParamTypeKind.Short => "short",
            ParamTypeKind.Boolean => "boolean",
            _ => throw new ArgumentOutOfRangeException()
        })}\"";
    }

    public string GetProxyUnboxer(MarshalingContext context, string source)
    {
        return $"Unbox<{GetCSharpExpression(false)}>({source})";
    }

    public string GetProxyBoxer(string source)
    {
        return $"Box({source})";
    }
}
