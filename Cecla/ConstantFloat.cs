// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Cecla;

public class ConstantFloat : IConstant
{
    internal ConstantFloat(ref BinaryReader reader)
    {
        var v = reader.ReadUnaligned<U4>().Value;
        Value = Unsafe.As<uint, float>(ref v);
    }

    public float Value { get; }
}
