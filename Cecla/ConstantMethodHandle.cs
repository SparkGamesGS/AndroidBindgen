// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantMethodHandle : IConstant, IHasReference
{
    private readonly ushort _referenceIndex;

    internal ConstantMethodHandle(ref BinaryReader reader)
    {
        ReferenceKind = reader.ReadUnaligned<byte>();
        _referenceIndex = reader.ReadUnaligned<U2>().Value;
    }

    public IConstant? Reference { get; private set; }
    public byte ReferenceKind { get; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Reference = classFile.ConstantPool.Span[_referenceIndex];
    }

    #endregion
}
