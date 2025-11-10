// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

namespace Cecla;

public class ConstantInteger : IConstant
{
    internal ConstantInteger(ref BinaryReader reader)
    {
        Value = reader.ReadUnaligned<U4>().Value;
    }

    public uint Value { get; }
}
