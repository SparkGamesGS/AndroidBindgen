// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal readonly struct ParameterTypeObject : IParameterType
{
    private readonly string _csharpExpression;

    public ParameterTypeObject(string className)
    {
        _csharpExpression = className.ToGlobalTypeName();
        ClassName = className;
    }

    public bool IsVoid => false;
    public string ClassName { get; }

    public string GetCSharpExpression(bool isNullable)
    {
        return isNullable ? $"{_csharpExpression}?" : _csharpExpression;
    }

    public string JavaArrayClassCSharpExpression =>
        $"global::AndroidBindgen.InternalBindings.java.lang.Class.arrayType({GetCSharpExpression(false)}.__ABG__Class)";

    public string CallMethodNameWithReturn(bool isStatic)
    {
        return isStatic ? "CallStaticObjectMethod" : "CallObjectMethod";
    }

    public string GetFieldMethodName(bool isStatic)
    {
        return isStatic ? "GetStaticObjectField" : "GetObjectField";
    }

    public string SetFieldMethodName(bool isStatic)
    {
        return isStatic ? "SetStaticObjectField" : "SetObjectField";
    }

    public string JValueInitializer(MarshalingContext context, string source)
    {
        return $"l = {source}?.GetRawObject() ?? global::System.IntPtr.Zero";
    }

    public string ToJavaArray(MarshalingContext? context, string source)
    {
        return
            $"global::UnityEngine.AndroidJNI.ToObjectArray({source}?.Select(element => element?.GetRawObject() ?? global::System.IntPtr.Zero)?.ToArray(), {GetCSharpExpression(false)}.__ABG__Class)";
    }

    public string FromJavaArray(string source)
    {
        return
            $"global::AndroidBindgen.AndroidJNIEx.FromObjectArray({source})?.Select({GetCSharpExpression(false)}.UnsafeFromRawObjectAndDeleteLocalRef)?.ToArray()";
    }

    public string ToCSharpValue(string source)
    {
        return $"{GetCSharpExpression(false)}.UnsafeFromRawObjectAndDeleteLocalRef({source})";
    }

    public string FromCSharpValue(string source)
    {
        return $"{source}?.GetRawObject() ?? global::System.IntPtr.Zero";
    }

    public string GetProxyParameterMatcher(string classNameSource, string classPointerSource, bool isBoxed)
    {
        return
            $"(nint){classPointerSource} == 0 || IsAssignableFrom({GetCSharpExpression(false)}.__ABG__Class, {classPointerSource})";
    }

    public string GetProxyUnboxer(MarshalingContext context, string source)
    {
        return $"{GetCSharpExpression(false)}.UnsafeFromRawObjectAndDeleteLocalRef({source})";
    }

    public string GetProxyBoxer(string source)
    {
        // TODO: dispose source
        return $"{source}?.GetRawObject() ?? global::System.IntPtr.Zero";
    }
}
