// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantNameAndType : IConstant, IHasReference
{
    private readonly ushort _descriptorIndex;
    private readonly ushort _nameIndex;

    internal ConstantNameAndType(ref BinaryReader reader)
    {
        _nameIndex = reader.ReadUnaligned<U2>().Value;
        _descriptorIndex = reader.ReadUnaligned<U2>().Value;
    }

    public string? Descriptor { get; private set; }

    public string? Name { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Name = classFile.ConstantPool.Span[_nameIndex].AsUtf8();
        Descriptor = classFile.ConstantPool.Span[_descriptorIndex].AsUtf8();
    }

    #endregion
}
