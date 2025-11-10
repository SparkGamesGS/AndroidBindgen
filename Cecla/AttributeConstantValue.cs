// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class AttributeConstantValue : IAttribute, IHasReference
{
    private readonly ushort _constantValueIndex;

    internal AttributeConstantValue(ref BinaryReader reader)
    {
        _constantValueIndex = reader.ReadUnaligned<U2>();
    }

    public IConstant? ConstantValue { get; private set; }

    #region IAttribute Members

    public string Name => "Signature";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        ConstantValue = classFile.ConstantPool.Span[_constantValueIndex];
    }

    #endregion
}
