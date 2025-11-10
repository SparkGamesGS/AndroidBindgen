// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class AttributeSignature : IAttribute, IHasReference
{
    private readonly ushort _signatureIndex;

    internal AttributeSignature(ref BinaryReader reader)
    {
        _signatureIndex = reader.ReadUnaligned<U2>();
    }

    public string? Signature { get; private set; }

    #region IAttribute Members

    public string Name => "Signature";

    #endregion

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        Signature = classFile.ConstantPool.Span[_signatureIndex].AsUtf8();
    }

    #endregion
}
