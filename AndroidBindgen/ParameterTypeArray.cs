// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AndroidBindgen;

internal readonly struct ParameterTypeArray : IParameterType
{
    public ParameterTypeArray(IParameterType elementType)
    {
        ElementType = elementType;
    }

    public IParameterType ElementType { get; }

    public bool IsVoid => false;

    public string GetCSharpExpression(bool isNullable)
    {
        return isNullable
            ? $"{ElementType.GetCSharpExpression(isNullable)}[]?"
            : $"{ElementType.GetCSharpExpression(isNullable)}[]";
    }

    public string JavaArrayClassCSharpExpression =>
        $"global::AndroidBindgen.InternalBindings.java.lang.Class.arrayType({ElementType.JavaArrayClassCSharpExpression})";

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
        var name = context.AllocLocalName();
        context.AppendPreCallLine($"var {name} = {ElementType.ToJavaArray(context, source)};");
        context.AppendPostCallLine($"{ElementType.FromJavaArray(name)}?.CopyTo({source}, 0);");
        return $"l = {name}";
    }

    // T[][] -> IntPtr
    public string ToJavaArray(MarshalingContext? context, string source)
    {
        if (context == null) return ToJavaArray(source);
        var name = context.AllocLocalName();
        context.AppendPreCallLine(
            $"var {name} = {source}?.Select(element => {ElementType.ToJavaArray(context, "element")})?.ToArray();");
        return
            $"global::UnityEngine.AndroidJNI.ToObjectArray({name}, {ElementType.JavaArrayClassCSharpExpression})";
    }

    private string ToJavaArray(string source)
    {
        return
            $"global::UnityEngine.AndroidJNI.ToObjectArray({source}?.Select(element => {ElementType.ToJavaArray(null, "element")})?.ToArray())";
    }

    // IntPtr -> T[][]
    public string FromJavaArray(string source)
    {
        return
            $"global::AndroidBindgen.AndroidJNIEx.FromObjectArray({source})?.Select(element => {ElementType.FromJavaArray("element")})?.ToArray()";
    }

    public string ToCSharpValue(string source)
    {
        return $"{ElementType.FromJavaArray(source)}";
    }

    public string FromCSharpValue(string source)
    {
        return ElementType.ToJavaArray(null, source);
    }

    public string GetProxyParameterMatcher(string classNameSource, string classPointerSource, bool isBoxed)
    {
        return
            $"(nint){classPointerSource} == 0 || IsArray({classPointerSource}) && {ElementType.GetProxyParameterMatcher($"GetBoxedTypeIdentifier(GetArrayElementType({classPointerSource}))", $"GetArrayElementType({classPointerSource})", false)}";
    }

    public string GetProxyUnboxer(MarshalingContext context, string source)
    {
        var name = context.AllocLocalName();
        context.AppendPreCallLine($"var {name} = {ElementType.FromJavaArray(source)};");
        context.AppendPostCallLine(
            $"if ({name} != null) ArrayCopy({ElementType.ToJavaArray(null, name)}, 0, {source}, 0, {name}!.Length);");
        return name;
    }

    public string GetProxyBoxer(string source)
    {
        return $"global::AndroidBindgen.AndroidJNIEx.ConvertToJNIArray({source})";
    }
}
