// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Cecla;

public class ConstantDouble : IConstant
{
    internal ConstantDouble(ref BinaryReader reader)
    {
        var v = reader.ReadUnaligned<U8>().Value;
        Value = Unsafe.As<ulong, double>(ref v);
    }

    public double Value { get; }
}
