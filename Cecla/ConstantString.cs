// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantString : IConstant, IHasReference
{
    private readonly ushort _stringIndex;

    internal ConstantString(ref BinaryReader reader)
    {
        _stringIndex = reader.ReadUnaligned<U2>().Value;
    }

    public string? String { get; private set; }

    #region IHasReference Members

    void IHasReference.ResolveReference(ClassFile classFile)
    {
        String = classFile.ConstantPool.Span[_stringIndex].AsUtf8();
    }

    #endregion
}
