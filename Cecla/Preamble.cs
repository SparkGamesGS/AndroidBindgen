// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Cecla;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Preamble : IEquatable<Preamble>
{
    public readonly U4 magic;
    public readonly U2 minorVersion;
    public readonly U2 majorVersion;

    public bool Equals(Preamble other)
    {
        return magic.Equals(other.magic) && minorVersion.Equals(other.minorVersion) &&
               majorVersion.Equals(other.majorVersion);
    }

    public override bool Equals(object? obj)
    {
        return obj is Preamble other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(magic, minorVersion, majorVersion);
    }

    public static bool operator ==(Preamble left, Preamble right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Preamble left, Preamble right)
    {
        return !left.Equals(right);
    }
}
