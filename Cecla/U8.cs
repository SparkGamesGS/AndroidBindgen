// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace Cecla;

public readonly struct U8 : IEquatable<U8>
{
    private readonly ulong _value;

    public ulong Value => !BitConverter.IsLittleEndian
        ? _value
        : (_value >> 56) |
          ((_value >> 40) & 0x000000000000FF00) |
          ((_value >> 24) & 0x0000000000FF0000) |
          ((_value >> 8) & 0x00000000FF000000) |
          ((_value << 8) & 0x000000FF00000000) |
          ((_value << 24) & 0x0000FF0000000000) |
          ((_value << 40) & 0x00FF000000000000) |
          ((_value << 56) & 0xFF00000000000000);

    private U8(ulong value)
    {
        _value = value;
    }

    public static implicit operator U8(ulong value)
    {
        return new U8(new U8(value).Value);
    }

    public static implicit operator ulong(U8 value)
    {
        return value.Value;
    }

    public bool Equals(U8 other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is U8 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(U8 left, U8 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(U8 left, U8 right)
    {
        return !left.Equals(right);
    }
}
