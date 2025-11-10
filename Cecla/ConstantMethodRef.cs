// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantMethodRef : IConstant, IHasReference
{
    private readonly ushort _classIndex;
    private readonly ushort _nameAndTypeIndex;

    internal ConstantMethodRef(ref BinaryReader reader)
    {
        _classIndex = reader.ReadUnaligned<U2>().Value;
        _nameAndTypeIndex = reader.ReadUnaligned<U2>().Value;
    }

    public ConstantClass? Class { get; private set; }
    public ConstantNameAndType? NameAndType { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Class = classFile.ConstantPool.Span[_classIndex].As<ConstantClass>();
        NameAndType = classFile.ConstantPool.Span[_nameAndTypeIndex].As<ConstantNameAndType>();
    }

    #endregion
}
