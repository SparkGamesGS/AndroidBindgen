// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantInvokeDynamic : IConstant, IHasReference
{
    private readonly ushort _nameAndTypeIndex;
    private ushort _bootstrapMethodAttrIndex;

    internal ConstantInvokeDynamic(ref BinaryReader reader)
    {
        _bootstrapMethodAttrIndex = reader.ReadUnaligned<U2>().Value;
        _nameAndTypeIndex = reader.ReadUnaligned<U2>().Value;
    }

    // public BootstrapMethodsAttribute bootstrapMethodAttr;
    public ConstantNameAndType? NameAndType { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        NameAndType = classFile.ConstantPool.Span[_nameAndTypeIndex].As<ConstantNameAndType>();
    }

    #endregion
}
