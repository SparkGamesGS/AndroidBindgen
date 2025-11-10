// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public class FieldInfo : IHasReference
{
    internal FieldInfo(ref BinaryReader reader, ReadOnlySpan<IConstant> unresolvedConstantPool)
    {
        AccessFlags = (AccessFlags)reader.ReadUnaligned<U2>().Value;

        var nameIndex = reader.ReadUnaligned<U2>().Value;
        Name = (unresolvedConstantPool[nameIndex] as ConstantUtf8)?.Value ??
               throw new InvalidClassException();

        var descriptorIndex = reader.ReadUnaligned<U2>().Value;
        Descriptor = (unresolvedConstantPool[descriptorIndex] as ConstantUtf8)?.Value ??
                     throw new InvalidClassException();

        var attributeCount = reader.ReadUnaligned<U2>().Value;
        var attributes = new IAttribute[attributeCount];
        Attributes = attributes;
        for (var i = 0; i < attributeCount; i++)
            attributes[i] = ClassFile.ReadAttribute(ref reader, unresolvedConstantPool);
    }

    public AccessFlags AccessFlags { get; }
    public ReadOnlyMemory<IAttribute> Attributes { get; }
    public string Descriptor { get; }
    public string Name { get; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        foreach (var attribute in Attributes.Span)
        {
            if (attribute is not IHasReference hasReference) continue;
            hasReference.ResolveReference(classFile);
        }
    }

    #endregion
}
