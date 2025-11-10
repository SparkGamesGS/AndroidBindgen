// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantClass : IConstant, IHasReference
{
    private readonly ushort _nameIndex;

    internal ConstantClass(ref BinaryReader reader)
    {
        _nameIndex = reader.ReadUnaligned<U2>().Value;
    }

    public string? Name { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Name = classFile.ConstantPool.Span[_nameIndex].AsUtf8();
    }

    #endregion
}
