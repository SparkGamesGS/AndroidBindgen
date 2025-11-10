// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public readonly struct U2 : IEquatable<U2>
{
    private readonly ushort _value;
    public ushort Value => !BitConverter.IsLittleEndian ? _value : (ushort)((_value >> 8) | (_value << 8));

    private U2(ushort value)
    {
        _value = value;
    }

    public static implicit operator U2(ushort value)
    {
        return new U2(new U2(value).Value);
    }

    public static implicit operator ushort(U2 value)
    {
        return value.Value;
    }

    public bool Equals(U2 other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is U2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(U2 left, U2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(U2 left, U2 right)
    {
        return !left.Equals(right);
    }
}
