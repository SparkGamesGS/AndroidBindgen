// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantLong : IConstant
{
    internal ConstantLong(ref BinaryReader reader)
    {
        Value = reader.ReadUnaligned<U8>().Value;
    }

    public ulong Value { get; }
}
