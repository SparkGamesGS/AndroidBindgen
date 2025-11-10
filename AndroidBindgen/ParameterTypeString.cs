// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal readonly struct ParameterTypeString : IParameterType
{
    public bool IsVoid => false;

    public string GetCSharpExpression(bool isNullable)
    {
        return isNullable ? "string?" : "string";
    }

    public string JavaArrayClassCSharpExpression =>
        "global::AndroidBindgen.InternalBindings.java.lang.Class.arrayType(global::AndroidBindgen.InternalBindings.java.lang.String.__ABG__Class)";

    public string CallMethodNameWithReturn(bool isStatic)
    {
        return isStatic ? "CallStaticStringMethod" : "CallStringMethod";
    }

    public string GetFieldMethodName(bool isStatic)
    {
        return isStatic ? "GetStaticStringField" : "GetStringField";
    }

    public string SetFieldMethodName(bool isStatic)
    {
        return isStatic ? "SetStaticStringField" : "SetStringField";
    }

    public string JValueInitializer(MarshalingContext context, string source)
    {
        return $"l = global::UnityEngine.AndroidJNI.NewString({source})";
    }

    // string[] -> IntPtr
    public string ToJavaArray(MarshalingContext? context, string source)
    {
        return
            $"global::UnityEngine.AndroidJNI.ToObjectArray({source}?.Select(str => global::UnityEngine.AndroidJNI.NewString(str))?.ToArray(), global::AndroidBindgen.InternalBindings.java.lang.String.__ABG__Class)";
    }

    // IntPtr -> string[]
    public string FromJavaArray(string source)
    {
        return
            $"global::AndroidBindgen.AndroidJNIEx.FromObjectArray({source})?.Select(ptr => global::UnityEngine.AndroidJNI.GetStringUTFChars(ptr))?.ToArray()";
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
        return $"{classNameSource} is null or \"java.lang.String\"";
    }

    public string GetProxyUnboxer(MarshalingContext context, string source)
    {
        return $"UnboxString({source})";
    }

    public string GetProxyBoxer(string source)
    {
        return $"BoxString({source})";
    }
}
