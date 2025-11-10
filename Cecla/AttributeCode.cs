// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class AttributeCode : IAttribute, IHasReference
{
    internal AttributeCode(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        MaxStack = reader.ReadUnaligned<U2>();
        MaxLocals = reader.ReadUnaligned<U2>();
        var codeLength = reader.ReadUnaligned<U4>().Value;
        Code = reader.Read((int)codeLength).ToArray();
        var exceptionTableLength = reader.ReadUnaligned<U2>().Value;
        var exceptionTable = new ExceptionTableEntry[exceptionTableLength];
        for (var i = 0; i < exceptionTableLength; i++)
            exceptionTable[i] = new ExceptionTableEntry(ref reader);
        ExceptionTable = exceptionTable;
        var attributeCount = reader.ReadUnaligned<U2>().Value;
        var attributes = new IAttribute[attributeCount];
        for (var i = 0; i < attributeCount; i++)
            attributes[i] = ClassFile.ReadAttribute(ref reader, unresolvedConstantPool);
        Attributes = attributes;
    }

    public ushort MaxStack { get; }
    public ushort MaxLocals { get; }
    public ReadOnlyMemory<byte> Code { get; }
    public ReadOnlyMemory<ExceptionTableEntry> ExceptionTable { get; }
    public ReadOnlyMemory<IAttribute> Attributes { get; }

    #region IAttribute Members

    public string Name => "Code";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        // avoid boxing
        static void ResolveReference<T>(T hasRef, ClassFile classFile) where T : IHasReference
        {
            hasRef.ResolveReference(classFile);
        }

        foreach (var entry in ExceptionTable.Span)
            ResolveReference(entry, classFile);
        foreach (var attribute in Attributes.Span)
            if (attribute is IHasReference hasRef)
                hasRef.ResolveReference(classFile);
    }

    #endregion
}
