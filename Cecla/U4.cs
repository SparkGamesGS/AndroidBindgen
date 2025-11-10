// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public readonly struct U4 : IEquatable<U4>
{
    private readonly uint _value;

    public uint Value => !BitConverter.IsLittleEndian
        ? _value
        : (_value >> 24) | ((_value >> 8) & 0xFF00) | ((_value << 8) & 0xFF0000) | (_value << 24);

    private U4(uint value)
    {
        _value = value;
    }

    public static implicit operator U4(uint value)
    {
        return new U4(new U4(value).Value);
    }

    public static implicit operator uint(U4 value)
    {
        return value.Value;
    }

    public bool Equals(U4 other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is U4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)_value;
    }

    public static bool operator ==(U4 left, U4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(U4 left, U4 right)
    {
        return !left.Equals(right);
    }
}
