// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantMethodType : IConstant, IHasReference
{
    private readonly ushort _descriptorIndex;

    internal ConstantMethodType(ref BinaryReader reader)
    {
        _descriptorIndex = reader.ReadUnaligned<U2>().Value;
    }

    public string? Descriptor { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Descriptor = classFile.ConstantPool.Span[_descriptorIndex].AsUtf8();
    }

    #endregion
}
