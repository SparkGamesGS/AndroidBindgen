// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class ClassFile
{
    internal ClassFile(ref BinaryReader reader)
    {
        Preamble = reader.ReadUnaligned<Preamble>();
        var constantPoolCount = reader.ReadUnaligned<U2>().Value;

        var constantPool = new IConstant[constantPoolCount];
        ConstantPool = constantPool;

        for (var i = 1; i < constantPoolCount; i++)
        {
            var tag = reader.ReadUnaligned<ConstantTag>();
            constantPool[i] = tag switch
            {
                ConstantTag.Utf8 => new ConstantUtf8(ref reader),
                ConstantTag.Integer => new ConstantInteger(ref reader),
                ConstantTag.Float => new ConstantFloat(ref reader),
                ConstantTag.Long => new ConstantLong(ref reader),
                ConstantTag.Double => new ConstantDouble(ref reader),
                ConstantTag.Class => new ConstantClass(ref reader),
                ConstantTag.String => new ConstantString(ref reader),
                ConstantTag.FieldRef => new ConstantFieldRef(ref reader),
                ConstantTag.MethodRef => new ConstantMethodRef(ref reader),
                ConstantTag.InterfaceMethodRef => new ConstantInterfaceMethodRef(ref reader),
                ConstantTag.NameAndType => new ConstantNameAndType(ref reader),
                ConstantTag.MethodHandle => new ConstantMethodHandle(ref reader),
                ConstantTag.MethodType => new ConstantMethodType(ref reader),
                ConstantTag.InvokeDynamic => new ConstantInvokeDynamic(ref reader),
                _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, "Unknown constant type")
            };

            if (tag is ConstantTag.Long or ConstantTag.Double) i++;
        }


        AccessFlags = (AccessFlags)reader.ReadUnaligned<U2>().Value;
        var thisClassIndex = reader.ReadUnaligned<U2>().Value;
        var superClassIndex = reader.ReadUnaligned<U2>().Value;
        ThisClass = ConstantPool.Span[thisClassIndex] as ConstantClass ?? throw new InvalidClassException();

        SuperClass = ConstantPool.Span[superClassIndex] as ConstantClass;

        var interfaceCount = reader.ReadUnaligned<U2>().Value;
        var interfaces = new ConstantClass[interfaceCount];
        Interfaces = interfaces;
        for (var i = 0; i < interfaceCount; i++)
            interfaces[i] = ConstantPool.Span[reader.ReadUnaligned<U2>().Value] as ConstantClass ??
                            throw new InvalidClassException();

        var fieldCount = reader.ReadUnaligned<U2>().Value;
        var fields = new FieldInfo[fieldCount];
        Fields = fields;
        for (var i = 0; i < fieldCount; i++)
            fields[i] = new FieldInfo(ref reader, ConstantPool.Span);

        var methodCount = reader.ReadUnaligned<U2>().Value;
        var methods = new MethodInfo[methodCount];
        Methods = methods;
        for (var i = 0; i < methodCount; i++)
            methods[i] = new MethodInfo(ref reader, ConstantPool.Span);

        var attributeCount = reader.ReadUnaligned<U2>().Value;
        var attributes = new IAttribute[attributeCount];
        Attributes = attributes;
        for (var i = 0; i < attributeCount; i++)
            attributes[i] = ReadAttribute(ref reader, ConstantPool.Span);

        foreach (var constant in ConstantPool.Span)
        {
            if (constant is not IHasReference hasReference) continue;
            hasReference.ResolveReference(this);
        }

        foreach (var field in Fields.Span)
            ((IHasReference)field).ResolveReference(this);

        foreach (var method in Methods.Span)
            ((IHasReference)method).ResolveReference(this);

        foreach (var attribute in Attributes.Span)
        {
            if (attribute is not IHasReference hasReference) continue;
            hasReference.ResolveReference(this);
        }
    }

    public AccessFlags AccessFlags { get; }
    public ReadOnlyMemory<IAttribute> Attributes { get; }
    public ReadOnlyMemory<IConstant> ConstantPool { get; }
    public ReadOnlyMemory<FieldInfo> Fields { get; }
    public ReadOnlyMemory<ConstantClass> Interfaces { get; }
    public ReadOnlyMemory<MethodInfo> Methods { get; }
    public Preamble Preamble { get; }
    public ConstantClass? SuperClass { get; }
    public ConstantClass ThisClass { get; }

    internal static IAttribute ReadAttribute(ref BinaryReader reader,
        ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var nameIndex = reader.ReadUnaligned<U2>().Value;
        if (unresolvedConstantPool[nameIndex] is not ConstantUtf8 constantUtf8)
            throw new InvalidClassException();

        var name = constantUtf8.Value;

        var length = reader.ReadUnaligned<U4>().Value;
        var attributeReader = new BinaryReader(reader.Read(checked((int)length)));

        return name switch
        {
            "ConstantValue" => new AttributeConstantValue(ref attributeReader),
            "RuntimeInvisibleAnnotations" => new AttributeRuntimeInvisibleAnnotations(ref attributeReader,
                unresolvedConstantPool),
            "RuntimeVisibleAnnotations" => new AttributeRuntimeVisibleAnnotations(ref attributeReader,
                unresolvedConstantPool),
            "RuntimeInvisibleParameterAnnotations" => new AttributeRuntimeInvisibleParameterAnnotations(
                ref attributeReader, unresolvedConstantPool),
            "Code" => new AttributeCode(ref attributeReader, unresolvedConstantPool),
            "LocalVariableTable" => new AttributeLocalVariableTable(ref attributeReader, unresolvedConstantPool),
            "Deprecated" => new AttributeDeprecated(),
            _ => new AttributeUnknown(ref attributeReader, name)
        };
    }

    internal static IElement ReadElement(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        var tag = (char)reader.ReadUnaligned<byte>();

        return tag switch
        {
            'B' or 'C' or 'D' or 'F' or 'I' or 'J' or 'S' or 'Z' or 's' => new ElementConstant(ref reader,
                (byte)tag),
            'e' => new ElementEnumConstant(ref reader),
            'c' => new ElementClass(ref reader),
            '@' => new ElementAnnotation(ref reader, unresolvedConstantPool),
            '[' => new ElementArray(ref reader, unresolvedConstantPool),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
